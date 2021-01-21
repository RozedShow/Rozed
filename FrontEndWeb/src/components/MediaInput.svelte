<script>
    import MediaType from '../MediaType'
    import {Button, ButtonGroup, Icon} from 'svelte-mui'
    import { onDestroy, onMount } from 'svelte';

    export let compacto = false
    export let media = {
        archivo: null,
        link: ""
    }
    export let videoUrl = null
    
    let vistaPreviaYoutube = ""
    
    let menuLink = false
    let archivoBlob = null
    let input = null
    let mediaType = MediaType.Imagen
    let el
    
    let inputLink = ""
    let estado = "vacio" // importarLink | cargado
    const youtubeRegex = /(?:youtube\.com\/\S*(?:(?:\/e(?:mbed))?\/|watch\?(?:\S*?&?v\=))|youtu\.be\/)([a-zA-Z0-9_-]{6,11})/

    async function actualizarArchivo() {
        if (input.files && input.files[0]) {
            archivoBlob = await getBlobFromInput(input)
            media.archivo = input.files[0]

            if(input.files[0].type.indexOf('image') != -1) {
                mediaType = MediaType.Imagen
            } else if(input.files[0].type.indexOf('video') != -1) { 
                mediaType = MediaType.Video
            }
        }
    }

    async function getBlobFromInput(input) {
        
        return new Promise((resolve, reject) => {
            if (!(input.files && input.files[0])) return null;
            let blob
            let reader = new FileReader()
            reader.onload = function (e) {
                blob = e.target.result
                resolve(blob)
            }
            reader.readAsDataURL(input.files[0])
        })
    }

    async function importarVideo() {
        let id = inputLink.match(youtubeRegex)
        console.log(id);
        if(!id) {
            inputLink = "Link invalido"
            return
        }
        mediaType = MediaType.Youtube
        vistaPreviaYoutube = `https://img.youtube.com/vi/${id[1]}/hqdefault.jpg`
        videoUrl = inputLink
        archivoBlob = `https://img.youtube.com/vi/${id[1]}/hqdefault.jpg`
        media.link = videoUrl
        estado = "cargado"

    }

    export function removerArchivo() {
        media.archivo = null
        media.link = ''
        archivoBlob = null
        input.value = ''
        inputLink = ''
        mediaType = MediaType.Imagen
        estado = "vacio"
        
    }
    
    onDestroy(() => {
        media.archivo = null
        archivoBlob = null
    })

    onMount(() => {
        window.addEventListener('paste', e => {
            console.log(e.clipboardData.files);
            input.files = e.clipboardData.files;
            actualizarArchivo()
        });
    })

</script>
<input 
    name="archivo" 
    on:change={actualizarArchivo}
    type="file" 
    id="hilo-input" 
    style="position: absolute; top:-1000px"
    bind:this={input}

>
<div 
    bind:this={el} 
    class:compacto class="video-preview media-input"
    style="{ (media.archivo || media.link)&& mediaType != MediaType.Video?`background:url(${archivoBlob || media})!important`: 'background:url(/imagenes/rose2.jpg)'};overflow:hidden;">

    {#if mediaType == MediaType.Video && media.archivo}
        <video src="{archivoBlob}"></video>
    {/if}

    {#if estado=="importarLink"}
    <div class="link-input">
        <input type="text" bind:value={inputLink} placeholder="Ingrese link de iutube">
            <Button  outlined shaped={true} on:click={importarVideo}> 
                    <icon>OK</icon>
            </Button>
            <Button  outlined shaped={true} on:click={() => estado = "vacio" }> 
                    <icon>x</icon>
            </Button>
    </div>
    {/if}
    {#if !media.archivo && estado=="vacio"}
        <span class="opciones">
            Agrega un archivo:
            <ButtonGroup>
                <Button on:click={() => input.click()} icon outlined shaped={true} on:click={() => true}> 
                     <icon class="fe fe-upload"></icon>
                </Button>
                <Button  icon outlined shaped={true} on:click={() => estado="importarLink"}> 
                     <icon class="fe fe-youtube"></icon>
                </Button>
            </ButtonGroup>
        </span>
        {/if}
    {#if media.archivo || media.link}
        <Button class="cancelar" on:click={removerArchivo} icon outlined shaped={true} on:click={() => true}> 
            <icon class="fe fe-x"></icon>
        </Button>
    {/if}
</div>

<style>
    .opciones {
        display: flex;
        align-items: center;
        width: 100%;
        justify-content: space-evenly;
        height: fit-content;
        margin-top: auto;
    }
    .media-input :global(.cancelar) {
        margin-left: auto;
        align-self: baseline;

    }

    .media-input {
        align-items: flex-end;
        min-height: 64px;
    }

    .compacto {
        height: auto;
        width: 100%;
        margin-left: 0;
        max-height: 300px;
    }

    .link-input {
        height: fit-content;
        display: flex;
        width: 100%;
    }

    video {
        max-height: 600px;
        max-width: 600px;
    }

    .media-input :global(.cancelar ){
        position: absolute;
        right: 16px;
        top: 16px;
    }
</style>