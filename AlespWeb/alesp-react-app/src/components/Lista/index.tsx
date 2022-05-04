import React from "react"
import Item from "./Item";

function Lista(){

    const tarefas = [{
        tarefa: 'React',
        tempo: '02:00:00'
    },{
        tarefa: 'JavaScript',
        tempo: '01:00:00'
    }]
     return(
<aside>
    <ul>{tarefas.map((item, index) => (
      <Item key={index} {...item} />
    ))}</ul>
</aside>
    );
}
    
export default Lista;
