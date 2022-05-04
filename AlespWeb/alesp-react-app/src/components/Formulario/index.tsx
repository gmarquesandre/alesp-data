import React from "react"
import Botao from "../Botao"
import style from './style.module.scss'


class Formulario extends React.Component{
    render(){
        return (
            <form className = {style.novaTarefa}>
                    <div className = "inputContainer">
                        <label htmlFor = "tarefa">Boa!</label>
                        <input className = "inputContainer" type ="text" name = "tarefa" id ="tarefa" placeholder="teste" required />
                    </div>
                <Botao texto="Testando"/>
            </form>
        )
    }
}

export default Formulario;