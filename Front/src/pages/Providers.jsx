
import React from 'react'
import { listProviders } from '../services/providers'
import { useAuth } from '../context/AuthContext'

export default function Providers(){
  const { logout } = useAuth()
  const providers = listProviders()
  return (
    <section className='card'>
      <div className='row' style={{justifyContent:'space-between'}}>
        <h2 style={{margin:0}}>Estado de Proveedores</h2>
        <button className='button' onClick={logout}>Cerrar sesión</button>
      </div>
      <table className='table'>
        <thead>
          <tr>
            <th>ID</th><th>Nombre</th><th>Disponibilidad</th><th>Rating</th><th>Costo/Km</th><th>ETA (min)</th>
          </tr>
        </thead>
        <tbody>
          {providers.map(p=> (
            <tr key={p.id}>
              <td>{p.id}</td>
              <td>{p.name}</td>
              <td><span className={`badge ${p.isAvailable?'ok':'busy'}`}>{p.isAvailable?'Disponible':'Ocupado'}</span></td>
              <td>{p.rating.toFixed(1)}</td>
              <td>{p.costPerKm.toFixed(2)}</td>
              <td>{p.eta}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  )
}
