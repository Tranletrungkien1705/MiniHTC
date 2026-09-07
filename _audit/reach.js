// Kha dat (reachability) tu [WebMethod] cua WS xuong biz — ban toi uu: quet token 1 lan/dong.
const fs=require('fs'), path=require('path');
const BIZ=process.argv[2], WS=process.argv[3], TARGETS=process.argv.slice(4);
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const callRe=/([A-Za-z_][A-Za-z0-9_]*)\s*\(/g;

function scan(dir){
  const out={};
  for(const f of fs.readdirSync(dir).filter(x=>x.endsWith('.cs'))){
    const lines=fs.readFileSync(path.join(dir,f),'utf8').split(/\r?\n/);
    const members=[]; lines.forEach((L,i)=>{const m=L.match(memberRe); if(m) members.push({line:i,name:m[1]});});
    out[f]={lines,members};
  }
  return out;
}
const biz=scan(BIZ), ws=scan(WS);
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
const roots=new Set();
for(const f in ws) for(const L of ws[f].lines){
  if(/^\s*\/\//.test(L)) continue;
  const m=L.match(/_biz\.([A-Za-z_][A-Za-z0-9_]*)\s*\(/);
  if(m && bizNames.has(m[1])) roots.add(m[1]);
}
const reach=new Set(roots); const q=[...roots];
while(q.length){ const c=q.shift(); const s=graph.get(c); if(!s) continue; for(const n of s) if(!reach.has(n)){ reach.add(n); q.push(n); } }
console.log('diemVaoWS='+roots.size+' hamBiz='+bizNames.size+' khaDat='+reach.size);
for(const t of TARGETS) console.log('  '+t+' => '+(reach.has(t)?'KHA DAT (co chay)':'KHONG kha dat (chet)'));
