// So SQL hai ham bat ky trong TERP.BizCarSv (dung cho cap LIVE-main vs _WH).
const fs=require('fs'),path=require('path');
const BIZ=process.argv[2], A=process.argv[3], B=process.argv[4];
function walk(d,a){for(const e of fs.readdirSync(d,{withFileTypes:true})){const fp=path.join(d,e.name);
 if(e.isDirectory()){if(!/^(bin|obj|Properties|Web References|Service References)$/.test(e.name))walk(fp,a);}
 else if(e.name.endsWith('.cs'))a.push(fp);}return a;}
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const bodies=new Map();
const files=walk(BIZ,[]);
console.log('  doc ' + files.length + ' file .cs');
for(const f of files){
  const lines=fs.readFileSync(f,'utf8').split(/\r?\n/); const mem=[];
  lines.forEach((L,i)=>{const m=L.match(memberRe); if(m&&!/^\s*\/\//.test(L)) mem.push({i,name:m[1]});});
  for(let k=0;k<mem.length;k++){
    const from=mem[k].i, to=(k+1<mem.length?mem[k+1].i:lines.length);
    if(!bodies.has(mem[k].name)) bodies.set(mem[k].name,{f,from,to,lines:lines.slice(from,to)});
  }
}
function sqlOf(o){
  return o.lines.map(x=>x.replace(/\r/g,''))
    .filter(x=>!/^\s*\/\/\//.test(x))   // #469: bo dong XML-doc ///
    .filter(x=>/select|from\s|where|join|group by|order by|into\s/i.test(x))
    .map(x=>x.replace(/\[@strDBName_CommonCenter\]\.\[dbo\]\./g,'')
             .replace(/_WH\b/g,'').replace(/_New\d{8}/g,'')
             .replace(/with\s*\(\s*nolock\s*\)/ig,'--//[mylock]')
             .replace(/--(?!\/\/\[mylock\]).*$/,'')
             .replace(/\(\s*1\s*=\s*1\s*\)/g,'1=1')
             .replace(/\s+/g,' ').trim().toLowerCase())
    .filter(x=>x.length>0);
}
for(const n of [A,B]){
  if(!bodies.has(n)){ console.log(n+' : KHONG CO'); continue; }
  const o=bodies.get(n);
  console.log(n+' : '+path.basename(o.f)+':'+(o.from+1)+'  sqlLines='+sqlOf(o).length);
}
if(!bodies.has(A)||!bodies.has(B)) process.exit(0);
const a=sqlOf(bodies.get(A)), b=sqlOf(bodies.get(B));
const sa=new Set(a), sb=new Set(b);
const onlyA=a.filter(x=>!sb.has(x)), onlyB=b.filter(x=>!sa.has(x));
console.log('chi o '+A+' = '+onlyA.length+'   chi o '+B+' = '+onlyB.length);
console.log('--- chi o '+A+':'); onlyA.slice(0,20).forEach(x=>console.log('  < '+x.slice(0,120)));
console.log('--- chi o '+B+':'); onlyB.slice(0,20).forEach(x=>console.log('  > '+x.slice(0,120)));
