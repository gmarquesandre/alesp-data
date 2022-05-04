import React from "react"
import Botao from "../Botao"
import './style.scss'

class Formulario extends React.Component{
    render(){
        return (
            <form className = "{style.novaTarefa}">
                    <div className = "inputContainer">
                        <label htmlFor = "tarefa">Boa!</label>
                        <input className = "inputContainer" type ="text" name = "tarefa" id ="tarefa" placeholder="teste" required />
                    </div>
                <Botao />
            </form>
        )
    }
}

export default Formulario;