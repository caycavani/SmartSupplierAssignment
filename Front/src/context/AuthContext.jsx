
import React, {createContext, useContext, useMemo, useState, useEffect} from 'react'

const AuthContext = createContext(null)

export function AuthProvider({children}){
  const [token,setToken]=useState(()=>localStorage.getItem('demo_token'))

  useEffect(()=>{ token ? localStorage.setItem('demo_token',token) : localStorage.removeItem('demo_token') },[token])

  const login = (user,pass)=>{
    if(user.trim()==='admin' && pass.trim()==='admin') { setToken('fake-jwt-token'); return {ok:true} }
    return {ok:false,message:'Credenciales inválidas (use admin/admin)'}
  }
  const logout = ()=> setToken(null)

  const value = useMemo(()=>({token,login,logout}),[token])
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth(){
  const ctx = useContext(AuthContext)
  if(!ctx) throw new Error('useAuth debe usarse dentro de <AuthProvider>')
  return ctx
}
