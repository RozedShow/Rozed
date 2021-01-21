<script>
    import {Dialog, Ripple, Button} from 'svelte-mui'
    import globalStore from '../../globalStore';
    import RChanClient from '../../RChanClient';
    import comentarioStore from '../Comentarios/comentarioStore';
    import Spinner from '../Spinner.svelte';

    export let encuesta
    export let hiloId
    export let votando = false

    let dialogo = false
    let estado = 0 // 0 no voto // 1 votando // 2 voto //3 detalles

    if(encuesta.haVotado || !$globalStore.usuario.estaAutenticado) estado = 2

    $: totalDeVotos = encuesta.opciones.map(e => e.votos).reduce((a,b) => a + b)

    function abrirEncuesta() {
        if(estado == 0) {
            estado = 1;
        } else if(estado == 2)
        {
            estado = 3
        }
    }

    async function votar(opcion) {
        try {
            votando = true
            let res = await RChanClient.votarEncuesta(hiloId, opcion)
            $comentarioStore = `[${opcion}]\n\n` + $comentarioStore
        } catch (error) {
            
        }

        encuesta.opciones.filter(o => o.nombre == opcion)[0].votos ++;
        encuesta = encuesta
        votando = false
        estado = 2
    }

    function calcularPorcentaje(opcion) {
        let votosTotales = 0
        encuesta.opciones.forEach(o => votosTotales+= o.votos);

        if(votosTotales == 0) votosTotales = 1
        
        const votosOpcion = encuesta.opciones.filter(o => o.nombre == opcion)[0].votos
        return ((votosOpcion / votosTotales) * 100).toFixed(2)
    }

    if(!$globalStore.usuario.estaAutenticado) estado = 2
    
</script>

<div class="encuesta">
    {#if encuesta}
        <div class="preview" on:click={abrirEncuesta}>
            <Ripple color="var(--color5)"></Ripple>
            {#each encuesta.opciones as o}
            <div class="opcion" title="{calcularPorcentaje(o.nombre)}% {o.nombre}" style="flex:{(estado < 2)?1:o.votos}">
                    <span >
                        {o.nombre}
                    </span>
                </div>
            {/each}
        </div>
    
        {#if estado == 1}
            <Dialog  visible={true} modal={true}>
                <div slot="title">Elija una opcion padre</div>
                <ul>
                    <Spinner cargando={votando}>
                        {#each encuesta.opciones as o}
                            <li on:click={() =>  votar(o.nombre)}>{o.nombre} <Ripple/></li>
                        {/each}
                    </Spinner>
                </ul>
            </Dialog>
        {/if}
        {#if estado == 3}
            <Dialog  visible={true} modal={true}>
                <div slot="title">Resultados ({totalDeVotos} {(totalDeVotos != 1)?'votos':'voto'})</div>
                <ul>
                    {#each encuesta.opciones as o}
                        <li>{calcularPorcentaje(o.nombre)}% {o.nombre} </li>
                    {/each}
                </ul>
                <div slot="actions" class="actions center">
                    <Button color="primary" on:click={() => estado = 2}>Ok</Button>
                </div>
            </Dialog>
        {/if}
    {/if}

</div>


<style>
    .encuesta {
        margin: 0 10px;
    }
    .preview {
        width: 100;
        display: flex;
        transition: 0.1s ease-in-out;
        border-radius: 4px;
        overflow: hidden;
        transition: 0.2s;
        background: var(--color1);
    }
    .preview:hover {
        /* height: 1rem; */
    }
    .opcion {
        height: 100%;
        flex: 1;
        position: relative;
        transition: 0.2s;
        min-width: 0;
        overflow: hidden;
        white-space: nowrap;
        padding: 4px;
        background: var(--color4);
        border-right: 1px solid var(--color5);
        border-left: 1px solid var(--color5);
    }
    .opcion:last-child {
        border-right: none;
    }
    .opcion:nth-child(2){
        border-left: none;
    }
    /* .opcion:nth-child(1) {background: #d43328;} 

    .opcion:nth-child(2) {background: #53a538;}

    .opcion:nth-child(3) {background: #ffc400;}

    .opcion:nth-child(4) {background: #00408a;}

    .opcion:nth-child(5) {background: #ff74c1;}
    
    .opcion:nth-child(6) {background: #000000;} */

    li {
        user-select: none;
        cursor: pointer;
        padding: 10p;
        padding: 8px 4px;
        border-bottom: dashed 1px var(--color7);
    }

    li:hover {
        background-color: rgba(9, 255, 0, 0.308);
    }

    .opcion {
        flex: 2 1 0%;
        text-align: center;
        color: var(--color-texto1);
        /* font-weight: 600; */
        user-select: none;
    }
</style>