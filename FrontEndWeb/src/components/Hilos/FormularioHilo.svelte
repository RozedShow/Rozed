<script>
import { fade, blur, fly } from 'svelte/transition';
import { Button, Ripple, Checkbox} from 'svelte-mui';
import config from "../../config";
import RChanClient from '../../RChanClient';
import ErrorValidacion from '../ErrorValidacion.svelte';
import MediaType from '../../MediaType'
import MediaInput from '../MediaInput.svelte';
import Captcha from '../Captcha.svelte';
import Spinner from '../Spinner.svelte';
import globalStore from '../../globalStore';
import FormularioEncuesta from './FormularioEncuesta.svelte';
import { tick } from 'svelte';

export let mostrar = false

let titulo = ""
let categoria = "-1"
let contenido = ""
let media
let captcha = ""

let encuesta = new Set([])
$: encuestaArray = [...encuesta]
$: console.log(encuesta)

let mostrarRango = false
let mostrarNombre = false

let cargando = false


let error = null

async function crear() {
    cargando = true
    encuesta = encuesta
    console.log(encuesta);
    try {
        let r = null 
        if(!$globalStore.usuario.esMod){
                r = await  await RChanClient
                    .crearHilo(titulo, 
                        categoria,
                        contenido, 
                        media.archivo, 
                        media.link, 
                        captcha,
                        [...encuesta])
            } else {
                r = await  await RChanClient
                    .crearHilo(titulo, 
                        categoria,
                        contenido, 
                        media.archivo, 
                        media.link,
                        captcha,
                        [...encuesta], 
                        mostrarNombre, 
                        mostrarRango)
            }
        if (r.status == 201) {
                window.location.replace(r.headers.location)
            }
    } catch (e) {
       error = e.response.data
    }
    cargando = false
}


let escribiendoRedactazo = false
let textarea2

async function onTeaxtAreaFocus(params) {
    console.log(textarea2);
    escribiendoRedactazo = true
    await tick()
    textarea2.focus()
}
</script>

{#if mostrar}
<div  transition:fly={{duration:200}}  class="sombra" style="position:fixed;left:0;top:0">
    <form  
        id="crear-hilo-form" 
        class="formulario crear-hilo panel"
        on:submit|preventDefault
    >

        <MediaInput bind:media={media}></MediaInput>

        <!-- svelte-ignore a11y-autofocus -->
        <input autofocus bind:value={titulo} name="titulo" placeholder="Titulo">

        <select bind:value={categoria}  name="categoria">
            <option value="-1" selected="selected" disabled="disabled">Categoría</option>
            {#each config.categorias as c}
                <option value="{c.id}">{c.nombre}</option>
            {/each}
        </select>

        <FormularioEncuesta bind:opciones={encuesta}/>

        <textarea 
            rows="15" 
            style="background: var(--color4);" 
            bind:value={contenido} name="contenido" 
            on:focus={onTeaxtAreaFocus}
            placeholder="Escribi un redactazo..."></textarea>
        {#if escribiendoRedactazo}
        <div class="">
                <textarea 
                    rows="10" 
                    style="background: var(--color4);" 
                    bind:this={textarea2}
                    bind:value={contenido} name="contenido" 
                    class="expandida"
                    on:focus={onTeaxtAreaFocus}
                    on:blur={() => escribiendoRedactazo = false}
                    placeholder="Escribi un redactazo..."></textarea>
                    <div style="position:absolute; z-index:2; bottom:60px;right:10px">
                        <Button color="var(--color5)" icon ><icon class="fe fe-check"></icon></Button>
                    </div>
            </div>
        {/if}

        <ErrorValidacion {error}/>

        {#if $globalStore.usuario.esMod}
            <div style=" flex-direction:row; display:flex">
                <Checkbox bind:checked={mostrarRango} right>Tag_Mod</Checkbox>
                <Checkbox bind:checked={mostrarNombre} right>Nombre</Checkbox>
            </div>
        {/if}
        <Captcha visible={config.general.captchaHilo}  bind:token={captcha}/>
        <div style="display:flex;     justify-content: flex-end;">
            <Button color="primary" on:click={() => mostrar = false}>Cancelar</Button>
                <Button color="primary" disabled={cargando} on:click={crear} >
                    <Spinner {cargando}>
                        Crear
                    </Spinner>
                </Button>
            <input type="submit" style="display:none">
        </div>
    </form>

</div>
{/if}

<style>
    .nav-boton {
        padding: 12px;
        align-items: center;
        display:flex;
        cursor: pointer;
    }

    video {
        width: 486px;
        height: 319px;
        top: -10px;
        position: absolute;
    }
    form {
        border-top: 2px solid var(--color5);
        max-height: 80vh !important;
        overflow-y: auto;
        max-width: unset;
        width: fit-content;
        position: relative;
        width: 600px;
    }
    @media (min-width: 600px) {
    form {
            min-width: 600px;
        }
    }
    @media (max-width: 400px) {
    form {
            width: 100%;
            height: 94vh;
        }
    }

    @media (max-width: 600px) {
        form {
            width: 100vw !important;
            height: calc(100vh - 50px) !important;
            max-height: calc(100vh - 50px) !important;
        }
    }

    .expandida {
        position: absolute;
        top: 10px;
        top: 10px;
        bottom: 50px;
        z-index: 1;
        width: calc(100% - 20px);
        height: calc(100% - 70px);
    }
</style>