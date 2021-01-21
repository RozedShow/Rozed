<script>
    import HiloCuerpo from "./Hilos/HiloCuerpo.svelte"
    import {Button} from "svelte-mui"
    import Tiempo from "./Tiempo.svelte"
    import Comentario from "./Comentarios/Comentario.svelte"
    import RChanClient from "../RChanClient";
    import { createEventDispatcher } from 'svelte';
    import HiloPreviewMod from "./Moderacion/HiloPreviewMod.svelte";
    import {EstadoDenuncia, MotivoDenuncia} from "../enums"
import globalStore from "../globalStore";

	const dispatch = createEventDispatcher()

    export let denuncia
    let {hilo, comentario, usuario}  = denuncia
    
    usuario = usuario ||{id:'', userName:'Anonimo'}

    $: rechazada = denuncia.estado == EstadoDenuncia.Rechazada
    $: aceptada = denuncia.estado == EstadoDenuncia.Aceptada

    hilo.cantidadComentarios = ""

    let mostrarVistaPrevia = false

    const motivos = Object.keys(MotivoDenuncia)

    async function rechazar() {
        try {
            let res = await RChanClient.rechazarDenuncia(denuncia.id)
            dispatch("rechazar", denuncia.id)
            denuncia.estado = 1;
            
            
        } catch (error) {
            
        }
    }
</script>

<div class="denuncia" class:rechazada  class:aceptada>
    <div class="header">
        <span style="background:var(--color2); padding:2px; border-radius: 4px">
            <Tiempo date={denuncia.creacion}/>
        </span>
        {#if $globalStore.usuario.esMod}
            <a class="userlink" href="/Moderacion/HistorialDeUsuario/{usuario.id}">{usuario.userName}</a>
            denuncio a 
            {#if denuncia.tipo == 0}
                <a class="userlink" href="/Moderacion/HistorialDeUsuario/{hilo.usuario.id}">{hilo.usuario.userName}</a>
            {:else}
                <a class="userlink" href="/Moderacion/HistorialDeUsuario/{comentario.usuario.id}">{comentario.usuario.userName}</a>
            {/if}
        {:else}
            <span class="userlink" href="/#">Gordo</span>
            denuncio a 
            <span class="userlink" href="/#">Gordo</span>
        {/if}
          por {motivos[denuncia.motivo]} {denuncia.aclaracion? `(${denuncia.aclaracion})`:''}
    </div>

    <div class="body">
        <Button  dense on:click={() => mostrarVistaPrevia = !mostrarVistaPrevia}>Previsualizar</Button>
        <!-- <a href="/Hilo/{hilo.id}#{comentario?.id}"></a> -->
        <a href="/Hilo/{hilo.id}#{comentario && comentario.id?comentario.id: ''}">
            <Button dense  >Ir</Button>
        </a>
        <Button dense  on:click={rechazar}>Rechazar</Button>
        {#if denuncia.tipo == 0}
            <HiloPreviewMod {hilo}/>
        {:else}
            <Comentario comentario={comentario}/>
        {/if}
        {#if mostrarVistaPrevia}
            <div class="vista-previa" on:mouseleave={() => mostrarVistaPrevia = false}>
                <HiloCuerpo {hilo}/>
            </div>
        {/if}
    </div>
</div>

<style>
    .header {
        padding: 8px;
    }
    .denuncia {
        position: relative;
        background: rgba(18, 18, 116, 0.315);
        margin-bottom: 16px;
        border-radius:  4px;
    }
    .vista-previa {
        background: var(--color2);
        position: absolute;
        z-index: 2;
        top: 30px;
        right: 10px
    }

    .userlink {
        background: var(--color6);
        color:black;
        padding:  2px 8px;
        border-radius: 4px;
    }
    .userlink:hover {
        text-decoration: underline;
        filter: brightness(1.4);
    }

    .rechazada {
        background: grey;
    }
    .aceptada {
        background: rgb(54, 153, 45);
    }
</style>