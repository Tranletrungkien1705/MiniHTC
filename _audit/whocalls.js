const fs=require('fs'), path=require('path');
const DIR=process.argv[2], FN=process.argv[3];
const files=fs.readdirSync(DIR).filter(f=>f.endsWith('.cs'));
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const callRe=new RegExp('\\b'+FN+'\\s*\\(');
for(const f of files){
  const lines=fs.readFileSync(path.join(DIR,f),'utf8').split(/\r?\n/);
  const members=[]; lines.forEach((L,i)=>{const m=L.match(memberRe); if(m) members.push({line:i,name:m[1]});});
  lines.forEach((L,i)=>{
    if(!callRe.test(L)) return;
    if(/^\s*\/\//.test(L)) return;
    if(memberRe.test(L)) return;
    let enc=null; for(const m of members){ if(m.line<=i) enc=m; else break; }
    console.log(f+':'+(i+1)+'  <= '+(enc?enc.name+'  (dong '+(enc.line+1)+')':'???'));
  });
}
