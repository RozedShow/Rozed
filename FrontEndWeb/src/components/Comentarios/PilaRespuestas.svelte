<script>
    import Comentario from "./Comentario.svelte";
    import {fly} from "svelte/transition"
    import {Button} from 'svelte-mui'

    export let diccionarioRespuestas = {}
    export let diccionarioComentarios = {}

    let diccionarioComentariosMap = new Map(Object.keys(diccionarioComentarios).map(k => [k, diccionarioComentarios[k]]))
    let diccionarioRespuestasMap = new Map(Object.keys(diccionarioRespuestas).map(k => [k, diccionarioRespuestas[k]]))

    export let historial = []

    function atras() {
        historial.pop()
        historial = historial
    }

    function agregarComentariosAPila(comentarios) {
        comentarios = comentarios.filter(c => c && c.id)
        comentarios = [...new Set(comentarios)]
        historial = [...historial, comentarios]
    }

</script>
{#if historial.length != 0}
    <div class="fondo" on:click={atras}>
        <div transition:fly={{duration:200, x:100}} class="pila-respuestas" on:click|stopPropagation>
            <div class="acciones">
                <Button on:click={atras}>Atras</Button>
                <Button on:click={() => historial = []}>Cerrar</Button>
            </div>
            <ul>
                {#each historial[historial.length - 1] as c (c.id)}
                    <li>
                        <Comentario
                            prevenirScroll = {true}
                            comentario={c}
                            respuetasCompactas={true}
                            on:tagClickeado={(e) => agregarComentariosAPila([diccionarioComentariosMap.get(e.detail)])}
                            on:motrarRespuestas={(e) => agregarComentariosAPila(diccionarioRespuestasMap.get(e.detail).map(  id => diccionarioComentariosMap.get(id)))}/>
                    </li>
                {/each}
            </ul>
        </div>
    </div>
{/if}

<style>
    .pila-respuestas {
        background: var(--color1);
        min-height: 100px;
        max-height: calc(100vh - 72px);
        width: 600px;
        max-width: calc(100vw - 16px);
        border-top: solid 2px var(--color5);
    }
    .pila-respuestas ul {
        overflow: hidden;
        overflow-y: scroll;
        max-height:calc(100vh - 72px);
    }
    .fondo {
        position: fixed;
        width: 100vw;
        height: 100vh;
        top: 0;
        left: 0;
        z-index: 9;
        background: rgba(16, 16, 17, 0.404);
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .acciones {
        padding: 4px;
        display: flex;
        justify-content: space-evenly;
        background: var(--color4);
    }

    .acciones :global(button){
        flex:1;
    }
</style>