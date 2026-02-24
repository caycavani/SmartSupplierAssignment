
import React from 'react'
import { Routes, Route, Navigate, useLocation } from 'react-router-dom'
import Header from '../components/Header'
import Login from './Login'
import Providers from './Providers'
import { useAuth } from '../context/AuthContext'

function Private({children}){
  const { token } = useAuth()
  const loc = useLocation()
  if(!token) return <Navigate to="/" state={{from:loc}} replace />
  return children
}

export default function App(){
  return (
    <>
      <Header />
      <main className='container'>
        <Routes>
          <Route path='/' element={<Login />} />
          <Route path='/providers' element={<Private><Providers/></Private>} />
          <Route path='*' element={<Navigate to='/' replace />} />
        </Routes>
        <p className='footer'>© Demo • Sin backend, sólo datos simulados</p>
      </main>
    </>
  )
}
