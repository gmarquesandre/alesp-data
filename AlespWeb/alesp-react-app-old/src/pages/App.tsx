import React from 'react';
import Formulario from '../components/Formulario';
import style from './style.module.scss';
import Lista from '../components/Lista';


function App() {
  return (
        <div  className = "{style.AppStyle}">
          <Formulario />
          <Lista />
        </div>
   
  );
}

export default App;
