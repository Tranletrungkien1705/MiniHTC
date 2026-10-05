// #382 Bo do GHI that su cho mot ten cot, theo NAM dang ghi da chot (#331/#334/#350):
//   1) Rows[0]["X"] =        2) <bien>["X"] =        3) alColumnEffective/alEffectiveColumn.Add("X")
//   4) SQL: update <alias> set ... X =                5) SQL: insert into <bang> (... X ...)
// KHONG tinh doc: drRow["X"].ToString(), , "Msg", drRow["X"] ...
const fs=require('fs'), path=require('path');
function readText(p){const b=fs.readFileSync(p);if(b.length>1&&b[0]===0xFF&&b[1]===0xFE)return b.toString('utf16le');return b.toString('utf8');}

// 🔴 #385 TEN HANG C# KHAC TEN COT DB — do duoc 387/4743 (8.2%) hang trong cac class Tbl*
//   cua DbDefine.cs. Vi du: hang `TblPMDetail.AmountAccum` tro toi cot that `PMPDAMOUNTTOTAL`;
//   `TblReportCongno.InvoiceAmountTotal` -> `RIT_TOTAL`. Moi phep audit theo TEN COT deu MU voi
//   nhung muc nay: do ten C# thi khong thay cho ghi nao, tuong la "dan xuat".
//   => Nay tu tra _audit/dbdefine_name_map.txt: mot cot duoc coi la CO GHI neu ten C# HOAC ten
//      cot DB tuong ung co cho ghi.
const cols0=fs.readFileSync(process.argv[2],'utf8').split(/\r?\n/).map(x=>x.trim()).filter(Boolean);
const alias={};
try{
  for(const ln of fs.readFileSync('d:/idocNet/_labs/MiniHTC-A/_audit/dbdefine_name_map.txt','utf8').split(/\r?\n/)){
    if(!ln||ln[0]==='#') continue;
    const p=ln.split('\t'); if(p.length<3) continue;
    (alias[p[1]]=alias[p[1]]||[]).push(p[2]);
  }
}catch(e){ console.log('(khong doc duoc dbdefine_name_map.txt — chay o che do chi TEN C#)'); }
const extra=[];
for(const c of cols0) for(const v of (alias[c]||[])) if(!cols0.includes(v)) extra.push(v);
const cols=cols0.concat(extra);
if(extra.length) console.log('(#385 them '+extra.length+' ten cot DB tuong ung: '+extra.join(', ')+')');
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
      // 5: insert into <bang> ( ... cot ... )
      //   🔴 #384 SUA AM TINH GIA: cua so nhin lai truoc day chi 6 dong. Danh sach cot cua
      //   `insert into` trong nguon nay moi cot MOT DONG, nen cot thu 8 tro di khong con thay
      //   `insert into` => bao "KHONG THAY GHI" oan. Vi du that: Auto_EstimateDelivery_BODtl
      //   (BizHTC.PlanDelivery.cs:2731) co QtyBOChuaXuatKho o dong thu 8 sau `insert into`.
      //   Nay: quet nguoc toi da 60 dong, DUNG lai khi gap `)` dong danh sach hoac `select`.
      let inIns=false;
      for(let k=i-1;k>=Math.max(0,i-60);k--){
        const p=lines[k];
        if(/^\s*\)/.test(p) || /\bselect\b/i.test(p)) break;
        if(/insert\s+into\s+/i.test(p)){ inIns=true; break; }
      }
      if(inIns && new RegExp('(^|[,(\\s])'+c+'([,)\\s]|$)').test(ln)){ r.sqlins++; if(r.where.length<3) r.where.push(path.basename(f)+':'+(i+1)); continue; }
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
  // #385: neu ten C# khong thay ghi NHUNG ten cot DB tuong ung CO ghi => ket luan la CO GHI.
  for(const v of (alias[c]||[])){
    const rv=res[v]; if(!rv) continue;
    const wv=rv.assign+rv.add+rv.sqlset+rv.sqlins;
    if(w===0 && wv>0) console.log(''.padEnd(28)+'=> THUC TE CO GHI qua ten cot DB '+v+' ('+rv.where.join(' ')+') — KHONG phai dan xuat.');
  }
}
