// #496: So phan C# (guard / hang / Convert / throw) giua ban chinh va ban _WH.
// Ly do: diffwh.js chi doc dong SQL => "SQL giong het" KHONG ket luan duoc "hai ban hanh xu nhu nhau"
// (ca #422/#495: SQL 9/9 giong het nhung than C# co 2 vs 0 lan HTC_WareHouse => chi mot ban kep moc ngay).
const fs=require('fs'),path=require('path');
const BIZ=process.argv[2], WS=process.argv[3];

function wsLiveNames(wsArg){
  const dirs=String(wsArg).split(',').map(x=>x.trim()).filter(Boolean);
  const out=new Set(); let files=0;
  const callRe=/_biz\.([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
  for(const d of dirs){ const st=[d];
    while(st.length){ const cur=st.pop();
      for(const e of fs.readdirSync(cur,{withFileTypes:true})){
        const fp=path.join(cur,e.name);
        if(e.isDirectory()){ if(!/^(bin|obj)$/.test(e.name)) st.push(fp); }
        else if(e.name.endsWith('.cs') && !/\.20\d{6}\.cs$/.test(e.name)){
          files++;
          for(const L of fs.readFileSync(fp,'utf8').split(/\r?\n/)){
            if(/^\s*\/\//.test(L)) continue;
            const m=L.match(callRe); if(m) out.add(m[1]);
          }
        }
      }
    }
  }
  console.log('  doc '+files+' file WS; ten biz duoc goi = '+out.size);
  return out;
}
function walk(d,a){for(const e of fs.readdirSync(d,{withFileTypes:true})){const fp=path.join(d,e.name);
 if(e.isDirectory()){if(!/^(bin|obj|Properties|Web References|Service References)$/.test(e.name))walk(fp,a);}
 else if(e.name.endsWith('.cs'))a.push(fp);}return a;}
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const files=walk(BIZ,[]);
console.log('  doc '+files.length+' file biz .cs');
const bodies=new Map();
for(const f of files){
  const lines=fs.readFileSync(f,'utf8').split(/\r?\n/); const mem=[];
  lines.forEach((L,i)=>{const m=L.match(memberRe); if(m&&!/^\s*\/\//.test(L)) mem.push({i,name:m[1]});});
  for(let k=0;k<mem.length;k++){
    const from=mem[k].i, to=(k+1<mem.length?mem[k+1].i:lines.length);
    if(!bodies.has(mem[k].name)) bodies.set(mem[k].name, lines.slice(from,to));
  }
}
const live=wsLiveNames(WS);

// Cac dau moc C# dang ke — dem tren dong KHONG phai comment
const MARKS=[
  ['throw',        /throw\s+CMyException|throw\s+CProcessException|CMyException\.Raise/],
  ['Check',        /\bmyCommon_Check|\bCheckExist|\bCheck[A-Z][A-Za-z]*\s*\(/],
  ['TConstHang',   /TConst\.[A-Za-z_.]+|Constants\.[A-Za-z_.]+/],
  ['Convert',      /\bConvert\.To[A-Za-z0-9]+\s*\(/],
  ['ifGuard',      /^\s*if\s*\(/],
  ['DateAdd',      /\.Add(Days|Months|Years|Hours|Minutes)\s*\(/],
  ['SaveData',     /\.SaveData\s*\(|ExecNonQuery\s*\(/],
];
function marks(lines){
  const c={};
  for(const [name] of MARKS) c[name]=0;
  for(const L of lines){
    if(/^\s*\/\//.test(L)) continue;
    // #503: bo dong #region/#endregion - khong phai ma chay nhung van khop mau Check*/TConst*
    if(/^[ \t]*#(region|endregion)\b/.test(L)) continue;
    for(const [name,re] of MARKS) if(re.test(L)) c[name]++;
  }
  return c;
}
const names=[...bodies.keys()];
const esc=x=>x.replace(/[.*+?^${}()|[\]\\]/g,'\\$&');
function cands(base,wh){
  return names.filter(n => wh
    ? (n===base+'_WH' || new RegExp('^'+esc(base)+'_WH_New\\d{6,8}$').test(n))
    : (n===base       || new RegExp('^'+esc(base)+'_New\\d{6,8}$').test(n)));
}
const bases=new Set();
for(const n of names){ const m=n.match(/^(.*)_WH(_New\d{6,8})?$/); if(m) bases.add(m[1]); }

let pairs=0, same=0; const diff=[];
for(const b of [...bases].sort()){
  // So TUNG cap cung "hau to" de khong lan kenh (luat #495)
  const ms=cands(b,false).filter(n=>live.has(n));
  const ws=cands(b,true).filter(n=>live.has(n));
  if(!ms.length||!ws.length) continue;
  // ghep theo hau to ngay neu co, khong thi ghep ban tran voi ban _WH tran
  for(const M of ms){
    const suf=(M.match(/_New(\d{6,8})$/)||[])[1];
    const W = suf ? ws.find(x=>x.endsWith('_WH_New'+suf)) : ws.find(x=>x===b+'_WH');
    if(!W) continue;
    pairs++;
    const a=marks(bodies.get(M)), c=marks(bodies.get(W));
    const d=MARKS.map(([n])=>[n,a[n],c[n]]).filter(([,x,y])=>x!==y);
    if(!d.length) same++;
    else diff.push({M,W,d});
  }
}
console.log('cap cung-kenh so duoc='+pairs+'  moc C# GIONG HET='+same+'  KHAC='+diff.length);
for(const x of diff) console.log('  KHAC: '+x.M+' vs '+x.W+'  ['+x.d.map(([n,a,b])=>n+' '+a+'/'+b).join(' · ')+']');
