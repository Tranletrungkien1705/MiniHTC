const fs=require('fs'), path=require('path');
const DIR=process.argv[2];
const files=fs.readdirSync(DIR).filter(f=>f.endsWith('.cs'));
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const src={}; const members={};   // file -> [{line, name}]
for(const f of files){
  const lines=fs.readFileSync(path.join(DIR,f),'utf8').split(/\r?\n/);
  src[f]=lines; members[f]=[];
  lines.forEach((L,i)=>{ const m=L.match(memberRe); if(m) members[f].push({line:i,name:m[1]}); });
}
// 1) moi ten ham dinh nghia co hau to xxx
const defs=new Set();
for(const f of files) for(const m of members[f]) if(/xxx$/.test(m.name)) defs.add(m.name);
// 2) voi moi ten, tim noi GOI (khong phai dong dinh nghia, khong phai comment)
function enclosing(f,i){ let r=null; for(const m of members[f]){ if(m.line<=i) r=m; else break; } return r; }
let called=0, liveCaller=0, deadOnly=0; const liveList=[];
for(const fn of [...defs].sort()){
  const callRe=new RegExp('\\b'+fn+'\\s*\\(');
  let any=false, hasLive=false;
  for(const f of files){
    src[f].forEach((L,i)=>{
      if(!callRe.test(L)) return;
      if(/^\s*\/\//.test(L)) return;              // dong comment
      if(memberRe.test(L)) return;                 // dong dinh nghia
      any=true;
      const enc=enclosing(f,i);
      if(enc && !/xxx$/.test(enc.name)) hasLive=true;
    });
  }
  if(any){ called++; if(hasLive){ liveCaller++; liveList.push(fn); } else deadOnly++; }
}
console.log('tenXxx='+defs.size+' conGoi='+called+' goiTuHamSONG='+liveCaller+' chiXxxGoi='+deadOnly);
console.log('DS song that: '+liveList.join(', '));
