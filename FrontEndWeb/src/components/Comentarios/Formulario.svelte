<script>
    import { createEventDispatcher } from 'svelte';
    import { Button, Checkbox} from 'svelte-mui'
    import comentarioStore from './comentarioStore'
    import RChanClient from '../../RChanClient'
    import ErrorValidacion from '../ErrorValidacion.svelte';
    import MediaInput from '../MediaInput.svelte';
    import Spinner from '../Spinner.svelte';
    import config from '../../config';
    import globalStore from '../../globalStore';

    let dispatch = createEventDispatcher()

    export let hilo

    let cargando = false

    $comentarioStore
    let media
    let mediaInput

    let mostrarRango = false
    let mostrarNombre = false

    let espera = 0
    $: if(espera != 0) {
        setTimeout(()=> espera--, 1000)
    }
    let error = null

    async function crearComentario() {
        if(espera != 0 || cargando) return

        try {
            cargando = true
            if($globalStore.usuario.esMod || $globalStore.usuario.esAuxiliar && config.general.modoSerenito){
                await RChanClient.crearComentario(hilo.id, $comentarioStore, media.archivo, media.link, "", mostrarNombre, mostrarRango)
            } else {
                await RChanClient.crearComentario(hilo.id, $comentarioStore, media.archivo, media.link)
            }
            if(!$globalStore.usuario.esMod) {
                espera = config.general.tiempoEntreComentarios
            }
            mediaInput.removerArchivo()
            dispatch("comentarioCreado")
        } catch (e) {
            error = e.response.data
            cargando = false
            return
        }
        
        cargando = false
        $comentarioStore = ''
        media.archivo = null
        error = null
    }
    function onFocus() {
        error = null
        if(!$globalStore.usuario.estaAutenticado) {
            window.location = '/Inicio'
        }
    }

</script>

<form on:submit|preventDefault="" id="form-comentario" class="form-comentario panel">
    <ErrorValidacion {error}/>

    <MediaInput bind:this={mediaInput} bind:media={media} compacto={true}></MediaInput>
    <textarea 
        on:focus={onFocus} 
        bind:value={$comentarioStore} 
        cols="30" 
        rows="10" placeholder="Que dificil discutir con pibes..."></textarea>

    <div class="acciones">
        {#if $globalStore.usuario.esMod || $globalStore.usuario.esAuxiliar && config.general.modoSerenito}
            <div style=" flex-direction:row; display:flex">
                <Checkbox bind:checked={mostrarRango} right>Lucesitas</Checkbox>
                <Checkbox bind:checked={mostrarNombre} right>Nombre</Checkbox>
            </div>
        {/if}
        <Button  disabled={cargando} color="primary"  class="mra" on:click={crearComentario}>
            <Spinner {cargando}>{espera == 0? 'Responder': espera}</Spinner>
        </Button>
    </div>
</form>


<style>
    .acciones :global(.mra) {
        margin-left: auto;
    }
    .acciones :global(i) {
        margin: 0 !important;
    }
    .form-comentario :global(.media-input) {
        margin-left: 0;
    }
</style>