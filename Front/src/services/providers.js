
export function listProviders(){
  const seed=[
    { id:1, name:'Grúa Express', isAvailable:true,  rating:4.6, costPerKm:2.4, lat:4.711, lng:-74.072 },
    { id:2, name:'Auxilio 24/7', isAvailable:false, rating:4.3, costPerKm:2.0, lat:4.651, lng:-74.05 },
    { id:3, name:'Rescate Andino', isAvailable:true, rating:4.8, costPerKm:2.7, lat:4.735, lng:-74.09 }
  ]
  const origin={lat:4.711,lng:-74.072}
  return seed.map(p=>({ ...p, eta: simulateEtaKm(km(origin,{lat:p.lat,lng:p.lng})) }))
}
function simulateEtaKm(distanceKm){ const v=35; const mins=Math.max(3,Math.round(distanceKm/v*60)); const jitter=Math.round(mins*0.15*(Math.random()-0.5)); return Math.max(2, mins+jitter) }
function km(a,b){ const R=6371, t=x=>x*Math.PI/180; const dLat=t(b.lat-a.lat), dLon=t(b.lng-a.lng); const l1=t(a.lat), l2=t(b.lat); const h=Math.sin(dLat/2)**2+Math.cos(l1)*Math.cos(l2)*Math.sin(dLon/2)**2; return 2*R*Math.asin(Math.sqrt(h)) }
