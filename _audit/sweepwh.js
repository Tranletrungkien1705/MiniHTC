// #467: Ban sao _WH co khac LUAT khong, hay chi khac CO SO DU LIEU?
// So phan SQL (chuan hoa) cua tung cap X_WH vs X.
const fs=require('fs'),path=require('path');
const BIZ=process.argv[2];
function walk(d,a){for(const e of fs.readdirSync(d,{withFileTypes:true})){const fp=path.join(d,e.name);
 if(e.isDirectory()){if(!/^(bin|obj|Properties|Web References|Service References)$/.test(e.name))walk(fp,a);}
 else if(e.name.endsWith('.cs'))a.push(fp);}return a;}
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;

const files=walk(BIZ,[]);
console.log('  doc ' + files.length + ' file .cs');
const bodies=new Map();   // ten -> mang dong
for(const f of files){
  const lines=fs.readFileSync(f,'utf8').split(/\r?\n/);
  const mem=[];
  lines.forEach((L,i)=>{const m=L.match(memberRe); if(m&&!/^\s*\/\//.test(L)) mem.push({i,name:m[1]});});
  for(let k=0;k<mem.length;k++){
    const from=mem[k].i, to=(k+1<mem.length?mem[k+1].i:lines.length);
    if(!bodies.has(mem[k].name)) bodies.set(mem[k].name, lines.slice(from,to));
  }
}
// Chuan hoa: chi giu dong SQL, bo tien to cheo-DB, bo _WH, gom khoang trang, ha chu thuong
function sqlOf(lines){
  return lines
    .map(x=>x.replace(/\r/g,''))
    .filter(x=>/select|from\s|where|join|group by|order by|into\s|update\s|insert\s/i.test(x))
    .map(x=>x.replace(/\[@strDBName_CommonCenter\]\.\[dbo\]\./g,'')
             .replace(/_WH\b/g,'')
             .replace(/with\s*\(\s*nolock\s*\)/ig,'--//[mylock]')
             .replace(/--(?!\/\/\[mylock\]).*$/,'')
             .replace(/\(\s*1\s*=\s*1\s*\)/g,'1=1')
             .replace(/\s+/g,' ').trim().toLowerCase())
    .filter(x=>x.length>0);
}
const whNames=[...bodies.keys()].filter(n=>/_WH$/.test(n));
let paired=0, same=0, diff=[];
for(const w of whNames){
  const base=w.replace(/_WH$/,'');
  if(!bodies.has(base)) continue;
  paired++;
  const a=sqlOf(bodies.get(base)).join('\n'), b=sqlOf(bodies.get(w)).join('\n');
  if(a===b) same++; else diff.push({w, la:sqlOf(bodies.get(base)).length, lb:sqlOf(bodies.get(w)).length});
}
console.log('ham _WH=' + whNames.length + '  co ban goc cung ten=' + paired
  + '  SQL GIONG HET=' + same + '  KHAC=' + diff.length);
for(const d of diff.slice(0,30)) console.log('  KHAC: ' + d.w + '  (goc ' + d.la + ' dong SQL / _WH ' + d.lb + ')');
