// #382 Bo do GHI that su cho mot ten cot, theo NAM dang ghi da chot (#331/#334/#350):
//   1) Rows[0]["X"] =        2) <bien>["X"] =        3) alColumnEffective/alEffectiveColumn.Add("X")
//   4) SQL: update <alias> set ... X =                5) SQL: insert into <bang> (... X ...)
// KHONG tinh doc: drRow["X"].ToString(), , "Msg", drRow["X"] ...
const fs=require('fs'), path=require('path');
function readText(p){const b=fs.readFileSync(p);if(b.length>1&&b[0]===0xFF&&b[1]===0xFE)return b.toString('utf16le');return b.toString('utf8');}

const cols=fs.readFileSync(process.argv[2],'utf8').split(/\r?\n/).map(x=>x.trim()).filter(Boolean);
const files=fs.readFileSync(process.argv[3],'utf8').split(/\r?\n/).map(x=>x.trim()).filter(Boolean);

const res={}; for(const c of cols) res[c]={assign:0,add:0,sqlset:0,sqlins:0,read:0,where:[]};

for(const f of files){
  if(/(\\|\/)(Delete\.[^\\\/]*|[^\\\/]* - Copy)\.cs$/i.test(f)) continue;   // file chet
  let txt; try{ txt=readText(f); }catch(e){ continue; }
  const lines=txt.split(/\r?\n/);
  for(let i=0;i<lines.length;i++){
    const ln=lines[i];
    if(ln.trim().startsWith('//')) continue;                                 // luat da chet (#305)
    for(const c of cols){
      if(ln.indexOf(c)<0) continue;
      const r=res[c];
      // 1+2: <bat ky>["X"] = (khong phai ==)
      if(new RegExp('\\["'+c+'"\\]\\s*=(?!=)').test(ln)){ r.assign++; if(r.where.length<3) r.where.push(path.basename(f)+':'+(i+1)); continue; }
      // 3: .Add("X")
      if(new RegExp('(alColumnEffective|alEffectiveColumn)\\.Add\\("'+c+'"\\)').test(ln)){ r.add++; if(r.where.length<3) r.where.push(path.basename(f)+':'+(i+1)); continue; }
      // 4: SQL set  X =   (dong bat dau bang , hoac X, trong khoi update)
      if(new RegExp('(^|[,\\s.])'+c+'\\s*=\\s*[^=]').test(ln) && /^\s*[,]?\s*[A-Za-z0-9_.]+\s*=/.test(ln)){
        // chi tinh khi trong 12 dong truoc co 'update '
        const lo=Math.max(0,i-12);
        if(/\bupdate\s+[A-Za-z0-9_]+/i.test(lines.slice(lo,i).join('\n'))){ r.sqlset++; if(r.where.length<3) r.where.push(path.basename(f)+':'+(i+1)); continue; }
      }
      // 5: insert into ... (cot)
      const lo2=Math.max(0,i-6);
      if(/insert\s+into\s+/i.test(lines.slice(lo2,i+1).join('\n')) && new RegExp('(^|[,(\\s])'+c+'([,)\\s]|$)').test(ln)){ r.sqlins++; if(r.where.length<3) r.where.push(path.basename(f)+':'+(i+1)); continue; }
      r.read++;
    }
  }
}
console.log('cot'.padEnd(28)+'GAN  ADD  SQLSET SQLINS  DOC   => KET LUAN');
for(const c of cols){
  const r=res[c];
  const w=r.assign+r.add+r.sqlset+r.sqlins;
  console.log(c.padEnd(28)
    +String(r.assign).padEnd(5)+String(r.add).padEnd(5)+String(r.sqlset).padEnd(7)
    +String(r.sqlins).padEnd(7)+String(r.read).padEnd(6)
    +(w>0?'CO GHI ('+r.where.join(' ')+')':'KHONG THAY GHI'));
}
