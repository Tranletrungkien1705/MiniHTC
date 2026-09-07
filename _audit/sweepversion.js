// Quet: moi ham biz co HAU TO NGAY/PHIEN BAN, ham nao KHA DAT, ham nao CHET,
// va MiniHTC dang trich dan ham nao trong Program.cs.
const fs=require('fs'),path=require('path'),cp=require('child_process');
const BIZ=process.argv[2],WS=process.argv[3],PROG=process.argv[4];
function walk(d,a){for(const e of fs.readdirSync(d,{withFileTypes:true})){const fp=path.join(d,e.name);
 if(e.isDirectory()){if(!/^(bin|obj|Properties|Web References|Service References)$/.test(e.name))walk(fp,a);}
 else if(e.name.endsWith('.cs'))a.push(fp);}return a;}
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const callRe=/([A-Za-z_][A-Za-z0-9_]*)\s*\(/g;
function scan(dir){const o={};for(const f of walk(dir,[])){const lines=fs.readFileSync(f,'utf8').split(/\r?\n/);
 const mem=[];lines.forEach((L,i)=>{const m=L.match(memberRe);if(m)mem.push({line:i,name:m[1]});});o[f]={lines,mem};}return o;}
const biz=scan(BIZ),ws=scan(WS);
const files=Object.keys(biz); console.log('so file biz doc duoc = '+files.length);
const names=new Set(); for(const f of files) for(const m of biz[f].mem) names.add(m.name);
const graph=new Map();
for(const f of files){const{lines,mem}=biz[f];let mi=-1,cur=null;
 for(let i=0;i<lines.length;i++){while(mi+1<mem.length&&mem[mi+1].line<=i){mi++;cur=mem[mi].name;}
  const L=lines[i];if(/^\s*\/\//.test(L)||!cur||memberRe.test(L))continue;
  callRe.lastIndex=0;let m;while((m=callRe.exec(L))!==null){const n=m[1];if(n===cur||!names.has(n))continue;
   let s=graph.get(cur);if(!s){s=new Set();graph.set(cur,s);}s.add(n);}}}
const roots=new Set();
for(const f in ws)for(const L of ws[f].lines){if(/^\s*\/\//.test(L))continue;
 const m=L.match(/_biz\.([A-Za-z_][A-Za-z0-9_]*)\s*\(/);if(m&&names.has(m[1]))roots.add(m[1]);}
const reach=new Set(roots),q=[...roots];
while(q.length){const c=q.shift();const s=graph.get(c);if(!s)continue;for(const n of s)if(!reach.has(n)){reach.add(n);q.push(n);}}
const prog=fs.readFileSync(PROG,'utf8');
const suf=[...names].filter(n=>/(_New\d{6,}|_\d{8}$|\d{4}NC$|_V\d+$)/.test(n)).sort();
const cited=suf.filter(n=>prog.includes(n));
const citedDead=cited.filter(n=>!reach.has(n));
console.log('ham hau-to-phien-ban='+suf.length+'  kha dat='+suf.filter(n=>reach.has(n)).length+'  chet='+suf.filter(n=>!reach.has(n)).length);
console.log('MiniHTC trich dan='+cited.length+'  trong do CHET='+citedDead.length);
citedDead.forEach(n=>console.log('  CHET-nhung-duoc-trich: '+n));
