
import React, {useState} from 'react'
import { useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Login(){
  const { login, token } = useAuth()
  const nav = useNavigate()
  const loc = useLocation()
  const [user,setUser]=useState('')
  const [pass,setPass]=useState('')
  const [err,setErr]=useState('')

  const onSubmit = (e)=>{
    e.preventDefault()
    const res = login(user,pass)
    if(res.ok){
      const from = loc.state?.from?.pathname || '/providers'
      nav(from,{replace:true})
    } else setErr(res.message)
  }

  return (
    <section className='card' style={{maxWidth:480}}>
      <h2 style={{marginTop:0}}>Login</h2>
      <form onSubmit={onSubmit} className='row' style={{flexDirection:'column',alignItems:'stretch'}}>
        <input className='input' placeholder='usuario' value={user} onChange={e=>setUser(e.target.value)} />
        <input className='input' placeholder='password' type='password' value={pass} onChange={e=>setPass(e.target.value)} />
        <button className='button' type='submit'>Ingresar</button>
        {err && <div style={{color:'#fca5a5',fontSize:13}}>{err}</div>}
      </form>
      <p style={{color:'#94a3b8',fontSize:12,marginTop:8}}>Credenciales demo: <b>admin / admin</b></p>
    </section>
  )
}
