<script>
    import {ComentarioEstado} from "../../enums"
    import Comentario from "../Comentarios/Comentario.svelte";
    export let comentario

</script>
    <div 
        class:eliminado = {comentario.estado == ComentarioEstado.eliminado}
    >
        <Comentario hilo={{id: comentario.hiloId}} {comentario}/>
    </div>

<style>
    /* .eliminado {
        border-left: solid 2px red;
    } */
</style>