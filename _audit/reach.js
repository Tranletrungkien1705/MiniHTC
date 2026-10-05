// Kha dat (reachability) tu [WebMethod] cua WS xuong biz — ban toi uu: quet token 1 lan/dong.
const fs=require('fs'), path=require('path');
// #478: tap ham SONG phai gop CA BON project WS, khong chi HTCWSCarSv.
function wsLiveNames(wsArg){
  const dirs = String(wsArg).split(",").map(x=>x.trim()).filter(Boolean);
  const out = new Set(); let files = 0;
  const callRe = /\b_?biz\.([A-Za-z_][A-Za-z0-9_]*)\s*\(/;   // #519: them kenh ClientService
  const cmtRe = /^\s*\/\//;
  for (const d of dirs) {
    const stack=[d];
    while(stack.length){
      const cur=stack.pop();
      for(const e of fs.readdirSync(cur,{withFileTypes:true})){
        const fp=path.join(cur,e.name);
        if(e.isDirectory()){ if(!/^(bin|obj)$/.test(e.name)) stack.push(fp); }
        // #481 BO QUA BAN LUU TRU: file .asmx.<yyyymmdd>.cs KHONG nam trong <Compile Include> cua csproj
        //   (da kiem: HTCWSCarSv.csproj chi liet ke WSCarSv.asmx.cs) ⇒ khong duoc bien dich.
        else if(e.name.endsWith(".cs") && !/\.20\d{6}\.cs$/.test(e.name)){
          files++;
          for(const L of fs.readFileSync(fp,"utf8").split(/\r?\n/)){
            if(cmtRe.test(L)) continue;
            const m=L.match(callRe); if(m) out.add(m[1]);
          }
        }
      }
    }
  }
  console.log("  doc "+files+" file WS tu "+dirs.length+" project; ten biz duoc goi = "+out.size);
  return out;
}
const WSDIRS = true;

const BIZ=process.argv[2], WS=process.argv[3], TARGETS=process.argv.slice(4);
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const callRe=/([A-Za-z_][A-Za-z0-9_]*)\s*\(/g;

// #453: PHAI quet DE QUY - cay nguon co thu muc con (HCCIntergration, iCIC.KhieuNai, CampaignMarketing...)
function walk(dir,acc){
  for(const e of fs.readdirSync(dir,{withFileTypes:true})){
    const fp=path.join(dir,e.name);
    if(e.isDirectory()){ if(!/^(bin|obj|Properties|Web References|Service References)$/.test(e.name)) walk(fp,acc); }
    else if(e.name.endsWith('.cs')) acc.push(fp);
  }
  return acc;
}
function scan(dir){
  const out={};
  for(const f of walk(dir,[])){
    const lines=fs.readFileSync(f,'utf8').split(/\r?\n/);
    const members=[]; lines.forEach((L,i)=>{const m=L.match(memberRe); if(m) members.push({line:i,name:m[1]});});
    out[f]={lines,members};
  }
  return out;
}
const biz=scan(BIZ);
const bizNames=new Set();
for(const f in biz) for(const m of biz[f].members) bizNames.add(m.name);

const graph=new Map();
for(const f in biz){
  const {lines,members}=biz[f];
  let mi=-1, cur=null;
  for(let i=0;i<lines.length;i++){
    while(mi+1<members.length && members[mi+1].line<=i){ mi++; cur=members[mi].name; }
    const L=lines[i];
    if(/^\s*\/\//.test(L)) continue;
    if(!cur) continue;
    if(memberRe.test(L)) continue;
    callRe.lastIndex=0; let m;
    while((m=callRe.exec(L))!==null){
      const n=m[1];
      if(n===cur || !bizNames.has(n)) continue;
      let s=graph.get(cur); if(!s){ s=new Set(); graph.set(cur,s); }
      s.add(n);
    }
  }
}
const roots=new Set(); for(const n of wsLiveNames(WS)) if(bizNames.has(n)) roots.add(n);
const reach=new Set(roots); const q=[...roots];
while(q.length){ const c=q.shift(); const s=graph.get(c); if(!s) continue; for(const n of s) if(!reach.has(n)){ reach.add(n); q.push(n); } }
console.log('diemVaoWS='+roots.size+' hamBiz='+bizNames.size+' khaDat='+reach.size);
for(const t of TARGETS) console.log('  '+t+' => '+(reach.has(t)?'KHA DAT (co chay)':'KHONG kha dat (chet)'));
