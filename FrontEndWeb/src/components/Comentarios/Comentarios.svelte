<script>
    import { fly } from 'svelte/transition';
    import { Button } from 'svelte-mui'
    import Comentario from './Comentario.svelte'
    import Formulario from './Formulario.svelte'
    import globalStore from '../../globalStore'
    import DialogoReporte from '../Dialogos/DialogoReporte.svelte';
    import Signal from '../../signal'
    import CarpetaMedia from './CarpetaMedia.svelte';
    import { onMount, tick } from 'svelte';
    import PilaRespuestas from './PilaRespuestas.svelte';
    import config, {configStore} from '../../config'
    import ajustesConfigStore from '../Dialogos/ajustesConfigStore'
    import Spinner from '../Spinner.svelte';
    import { localStore} from '../../localStore'



    export let hilo
    export let comentarios
    let modoTelefono = $globalStore.esCelular
    let nuevosComentarios = []
    let comentariosStore = localStore("Comentarios", {modoVivo : false});

    function cargarNuevosComentarios() {
        comentarios = [...nuevosComentarios, ...comentarios]
        nuevosComentarios = []
        comentarios.forEach(agregarComentarioADiccionario)
        comentarios.forEach(cargarRespuestas)
        // Añadir el restag a los comentarios tageados por este comentario
        comentarios = comentarios;
    }

    let diccionarioRespuestas = {}
    let diccionarioComentarios = {}
    
    function agregarComentarioADiccionario(comentario) {
        diccionarioComentarios[comentario.id] = comentario
			let tags = comentario.contenido.match(/#([A-Z0-9]{8})/g)
			if(!tags) return;
			let id = comentario.id
			for(const tag of tags) {
				let otraId = tag.slice(1, 9)
				if(!diccionarioRespuestas[otraId]) diccionarioRespuestas[otraId] = []
                diccionarioRespuestas[otraId].push(id)
                diccionarioRespuestas[otraId] = diccionarioRespuestas[otraId]
            }
        diccionarioRespuestas = diccionarioRespuestas
    }

    function cargarRespuestas(comentario) {
        if(diccionarioRespuestas[comentario.id]) comentario.respuestas = [...diccionarioRespuestas[comentario.id]]
        else comentario.respuestas = []
        comentario.respuestas = Array.from(new Set(comentario.respuestas))
    }

	comentarios.forEach(agregarComentarioADiccionario)
	comentarios.forEach(cargarRespuestas)

    function onComentarioCreado(comentario) {
        nuevosComentarios = [comentario, ...nuevosComentarios]
        comentario.respuestas = []
        if($comentariosStore.modoVivo) cargarNuevosComentarios()
    }

     Signal.coneccion.on("NuevoComentario", onComentarioCreado)
     Signal.subscribirseAHilo(hilo.id)
     Signal.coneccion.on("ComentariosEliminados", ids => {
        comentarios = comentarios.filter(c => ! ids.includes(c.id))
        nuevosComentarios = nuevosComentarios.filter(c => ! ids.includes(c.id))
     })

    let resaltando = false;
    function resaltarComentariosDeUsuario(usuarioId) {
        if(!$globalStore.usuario.esMod) return;
        if(resaltando) {
            comentarios.forEach(c => c.resaltado = false)
            comentarios = comentarios   
            resaltando = false
            return;
        }
        comentarios.forEach(c => {
            if(c.usuarioId == c.usuarioId) {
                resaltando = true;
            }
            c.resaltado = usuarioId == c.usuarioId
        })
        comentarios = comentarios
    }

    function tagCliqueado(e) {
        if(!diccionarioComentarios[e.detail]) return;
        if(modoTelefono && !$ajustesConfigStore.tagClasico) {
            e.preventDefault() 
            historialRespuestas = [[diccionarioComentarios[e.detail]]]
        }
        comentarios.forEach(c => c.resaltado = false)
        comentarios = comentarios
        diccionarioComentarios[e.detail].resaltado = true;
    }
    

    let comentarioUrl = window.location.hash.replace("#", "")

    async function irAComentario(comentarioId) {
        if(diccionarioComentarios[comentarioId]) {
            diccionarioComentarios[comentarioId].resaltado = true
            
            let comentarioDOM = document.getElementById(comentarioId)
            await tick()
            if(comentarioDOM) comentarioDOM.scrollIntoView({block:'center'})
        }
    }

    onMount(() =>irAComentario(comentarioUrl))
    irAComentario(comentarioUrl)

    let resaltadoIdUnico = false
    function idUnicoClickeado(e) {
        comentarios.forEach(c => {
            if(!resaltadoIdUnico) {
                c.resaltado = c.idUnico == e.detail
            } else {
                c.resaltado = false
            }
        });
        comentarios = comentarios
        resaltadoIdUnico = !resaltadoIdUnico
    }
        
    let carpetaMedia = false
    let historialRespuestas = []

    let cargarComentarios = false;
    onMount(async() => setTimeout( async () => {
        cargarComentarios = true
        await tick();
        irAComentario(comentarioUrl)
    }, 120))
    
</script>
<CarpetaMedia {comentarios} bind:visible={carpetaMedia}></CarpetaMedia>
<div class="comentarios">
    <PilaRespuestas {diccionarioComentarios} {diccionarioRespuestas} historial = {historialRespuestas}/>
    {#if !$configStore.general.modoMessi || $globalStore.usuario.esMod}
        <Formulario {hilo} on:comentarioCreado={cargarNuevosComentarios}/>
    {/if}

    <div class="contador-comentarios panel">
        <h3>Comentarios ({comentarios.length}) 
        </h3>
        {#if nuevosComentarios.length != 0}
            <div class="badge" style="    font-size: 18px;height: auto;cursor: pointer;">
                <span on:click={cargarNuevosComentarios}>+ {nuevosComentarios.length}</span>
            </div>
        {/if}
        <div class="acciones-comentario">
            <!-- <i on:click={() => carpetaMedia = !carpetaMedia} class="fe fe-folder"></i> -->
            <Button on:click={() => $comentariosStore.modoVivo = !$comentariosStore.modoVivo} 
                title="Comentarios en vivo (se cargan automaticamente)"
                dense icon> <div class="boton-modo-vivo" style={$comentariosStore.modoVivo?'background:var(--color5);':'background:white'}></div>
            </Button>
            <Button on:click={() => carpetaMedia = !carpetaMedia} 
                dense icon><icon class="fe fe-folder"></icon>
            </Button>
            {#if comentarios.length > 0}
                <a href="#{comentarios[comentarios.length -1].id}" style="margin: 0;">
                    <Button
                        dense icon><icon class="fe fe-arrow-down"></icon>
                    </Button>
                </a>
            {/if}
        </div>
    </div>
    <div class="lista-comentarios">
        <Spinner cargando={!cargarComentarios}>
            {#each comentarios as comentario (comentario.id)}
                <li transition:fly|local={{y: -50, duration:250}}>
                    <Comentario 
                        on:colorClick={(e) => resaltarComentariosDeUsuario(e.detail.usuarioId || '') } 
                        hilo={hilo} 
                        bind:comentario bind:comentariosDic = {diccionarioComentarios}
                        respuetasCompactas={modoTelefono}
                        on:tagClickeado={tagCliqueado}
                        on:idUnicoClickeado={idUnicoClickeado}
                        on:motrarRespuestas={(e)=>historialRespuestas=[diccionarioRespuestas[e.detail].map(c => diccionarioComentarios[c])]}
                        />
                </li>
            {/each}
        </Spinner>
            
        </div>
        <div class="espacio-vacio"></div>

</div>

<style>
    .espacio-vacio {
        height: 200px;   
        /* scroll-snap-align: center; */
    }
    .boton-modo-vivo {
        width:8px;
        height:8px;
        border-radius:8px;
        transition: background 0.2s;
    }
    @media(max-width: 600px) {
        .espacio-vacio {height: 24px;}
    }
    :global(.loader) {
        margin: 0 auto;
    }

</style>