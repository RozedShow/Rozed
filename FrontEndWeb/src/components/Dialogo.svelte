<script>
    import ErrorValidacion from "./ErrorValidacion.svelte";
    import {Dialog, Button} from 'svelte-mui'
    import { onDestroy } from "svelte";

    export let titulo = "Accion"
    export let accion = () => console.log("Accionado")
    export let visible = false
    export let textoActivador = "Accion"

    let exito = false 
    let respuesta = null

    async function ejecutarAccion() {
        try {
            error  = null   
            respuesta = (await accion()).data
            exito = true;
            setTimeout(()=> visible=false, 1000) 
        } catch (e) {
            exito = false;
            error = e.response.data
        }
    }
    
    let error = null

    onDestroy(() => {
        respuesta = null
        error = null
    })
    $: if(visible == false ) {
        respuesta = null
        error = null
        exito = false
    }
</script>

<span on:click={() => visible = true}>
    <slot name="activador" >
        <Button >{textoActivador}</Button>
    </slot>
</span>

    <Dialog width="320" bind:visible={visible}>
        <div slot="title">{titulo}</div>
        <ErrorValidacion bind:error={error} />

        {#if exito}
            <p class="exito">{respuesta.mensaje}</p>
        {/if}
        
        <slot name="body">
    
        </slot>
        
        <div slot="actions" class="actions center">
            <Button color="primary" on:click={()=> visible = false}>Cancelar</Button>
            <Button color="primary" on:click={ejecutarAccion}>Aceptar</Button>
        </div>
    </Dialog>

<style>
    .exito {
        color: greenyellow;
    }

    :global(label .label-text) {
        white-space: break-spaces !important;
    }
</style>