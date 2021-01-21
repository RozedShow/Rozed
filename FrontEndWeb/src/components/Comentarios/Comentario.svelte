<script>
    import { onMount } from 'svelte'
    import { createEventDispatcher } from 'svelte';
    import {Menuitem, Button, Icon, Ripple } from 'svelte-mui';
    import Menu from '../Menu.svelte'
    import comentarioStore from './comentarioStore'
    import { fly } from 'svelte/transition';
    
    import Tiempo from '../Tiempo.svelte'
    import globalStore from '../../globalStore';
    import Media from '../Media.svelte';
    import {abrir} from '../Dialogos/Dialogos.svelte'
    import { ComentarioEstado, CreacionRango } from '../../enums';
    import selectorStore from '../Moderacion/selectorStore'

    export let comentario;
    export let hilo = { id:null };
    export let comentariosDic = {   };
    export let resaltado = false
    export let prevenirScroll = false
    export let respuetasCompactas = false

    export let esRespuesta = false

    comentario.estado = comentario.estado || ComentarioEstado.normal
    
    let el
    let mostrandoRespuesta = false
    let respuestaMostrada

    let mediaExpandido = false
    
    let windowsWidh = window.screen.width

    let visible = !$globalStore.comentariosOcultos.has(comentario.id)
    
    let dispatch = createEventDispatcher()
    
    onMount(() => {

        let respuestas = el.querySelectorAll(".restag")
        respuestas.forEach(r => {
            r.addEventListener("mouseover", () => mostrarRespuesta(r.getAttribute("r-id").trim()))
            r.addEventListener("mouseleave", ocultarRespuesta)
            r.addEventListener("click", (e) => {
                resaltarCliqueado(r.getAttribute("r-id").trim())
                if(prevenirScroll) {
                    console.log("Scroll prevenido");
                    e.preventDefault()
                }
            })
        })
    })

    function mostrarRespuesta(id) {
        if(!comentariosDic[id]) return
        mostrandoRespuesta = true
        respuestaMostrada = comentariosDic[id]
    }

    function resaltarCliqueado(id) {
        dispatch("tagClickeado", id)
    }

    function ocultarRespuesta() {
        mostrandoRespuesta = false
    }

    function toggle() {
        if(visible) {
            $globalStore.comentariosOcultos.set(comentario.id, true)
        } else {
            $globalStore.comentariosOcultos.delete(comentario.id)
        }
        $globalStore.comentariosOcultos = $globalStore.comentariosOcultos 
        visible = !visible
    }

    function seleccionar() {
        if(!$globalStore.usuario.esMod) return;
        selectorStore.selecionar(comentario.id)
    }

    if(!Array.isArray(comentario.respuestas)) comentario.respuestas = []

    function tagear(id) {
        if(!$comentarioStore.includes(`>>${comentario.id}\n`))
            $comentarioStore+= `>>${comentario.id}\n`
    }

    function esOp(comentarioId) {
        let  comentario = comentariosDic[comentarioId] || {esOp:false} //??quitado
        return comentario.esOp
    }

    function idUnicoColor() {
        let coloresPosibles = ['#7bd800', '#00d87e', '#006ad8','#3500d8', '#8500d8', '#d80096', '#737679', '#5d130b', '#ec64e2', '#ff5722']
        let n = comentario.idUnico.charCodeAt(0) + comentario.idUnico.charCodeAt(1) + comentario.idUnico.charCodeAt(2);
        return coloresPosibles[n % coloresPosibles.length - 1]
    }
</script>

<div bind:this={el}
    class:resaltado={comentario.resaltado  || resaltado|| $selectorStore.seleccionados.has(comentario.id)} 
    class="comentario {windowsWidh <= 400?"comentario-movil":""}" 
    class:eliminado = {comentario.estado == ComentarioEstado.eliminado}
    class:comentarioMod = {comentario.rango > CreacionRango.Auxliar}
    class:comentarioAuxiliar = {comentario.rango == CreacionRango.Auxliar}
    r-id="{comentario.id}" id="{comentario.id}{esRespuesta?'-res':''}"
    style={(comentario.respuestas.length > 0)?'padding-bottom: 20px': ''}
    >

    {#if  comentario.respuestas.length > 0}
        <div class="respuestas-compactas"
            on:click={() => dispatch("motrarRespuestas", comentario.id)}>
            R: {comentario.respuestas.length}
        </div>
        <div  class="respuestas">
            {#each comentario.respuestas as r }
            <a href="#{r}" class="restag" r-id="{r}"
                on:mouseover={() => mostrarRespuesta(r)}
                on:mouseleave={ocultarRespuesta}
            >&gt;&gt;{r}{esOp(r)?'(OP)' : ''} </a> 
            {/each}
        </div    >
    {/if}
    <div on:click={() => dispatch("colorClick", comentario)} 
        class="color color-{comentario.color} ns"
        class:dado={comentario.dados != undefined && comentario.dados != -1}
    >
        {#if comentario.dados!= undefined && comentario.dados != -1}
            {comentario.dados}
        {:else if comentario.rango}
            {CreacionRango.aString(comentario.rango).toUpperCase()}
        {:else}
            ANON
        {/if}
    </div>
    <div class="header">
        {#if comentario.esOp} <span class="nick tag tag-op">OP</span>{/if}
        <span 
            on:click={seleccionar}
            class:nombreResaltado = {comentario.nombre} 
            class="nick nombre cptr">{comentario.nombre ||'Gordo'}
        </span>
        {#if comentario.idUnico}
            <span 
                on:click={() => dispatch("idUnicoClickeado", comentario.idUnico)} 
                class="tag ns cpt idunico" style={`background:${idUnicoColor()};`}>
                {comentario.idUnico} <Ripple color="var(--color5)"/>
            </span>
        {/if}
        {#if comentario.usuarioId}
        <a href="/Moderacion/HistorialDeUsuario/{comentario.usuarioId}" style="color:var(--color6) !important">
            <span class="nick">{comentario.usuarioId.split("-")[0]}</span>
        </a>
        {/if}
        <!-- <span class="rol tag">anon</span> -->
        <span class="id tag ns" on:click={() => tagear(comentario.id)}>{comentario.id}</span>
        <span class="tiempo"><Tiempo date={comentario.creacion}/></span>

        <div>
            <Menu>
                <span slot="activador" on:click={() => mostrarMenu = true} class=""><i class="fe fe-more-vertical relative"></i></span>
                <li on:click={() => toggle()}>{visible?'Ocultar':'Mostrar'}</li>
                <li on:click={() => abrir.reporte(hilo.id || comentario.hiloId, comentario.id)}>Reportar</li>
                {#if $globalStore.usuario.esMod || $globalStore.usuario.esAuxiliar}
                    <hr>
                    {#if comentario.hiloId}
                        <a href="/Hilo/{comentario.hiloId}#{comentario.id}" style="color:white!important">
                            <Menuitem>Ir</Menuitem>
                        </a>
                    {/if}
                    <Menuitem on:click={() => abrir.ban(hilo.id || comentario.hiloId, comentario.id)} >Banear</Menuitem>
                    {#if comentario.estado == ComentarioEstado.normal}
                    <Menuitem on:click={() => abrir.eliminarComentarios([comentario.id])}>Eliminar</Menuitem>
                    {:else}
                        <Menuitem on:click={() => abrir.restaurarComentario(comentario.id)} >Restaurar</Menuitem>
                    {/if}
                {/if}
            </Menu>
        </div>

    </div>
    <div class="respuestas">
    </div>
    {#if visible}
        <div 
            class="contenido"
            class:mediaExpandido>
            {#if comentario.media}
                <Media media={comentario.media} bind:abierto={mediaExpandido}/>
            {/if}
            <span class="texto">
                {@html comentario.contenido}
            </span>
        </div>
    {/if}
    {#if mostrandoRespuesta}
        <div transition:fly|local={{x: -50, duration:150}} class="comentario-flotante">
            <svelte:self comentario = {respuestaMostrada}  esRespuesta={true}></svelte:self>
        </div>
    {/if}
</div>

<style>
    .comentario {
        display: grid;
        gap: 10px;
        grid-template-columns: 50px calc(100% - 60px);
        grid-template-areas:
            "color header"
            "color respuestas"
            "color cuerpo";
        grid-template-rows: auto auto auto;
        padding: 10px;
        position: relative;
        margin-bottom: 8px;
        text-underline-position: under;
        transition: 0.1s background-color linear;
        position: relative;
    }

    .comentario .contenido {
        grid-area: cuerpo;
    }
    .mediaExpandido {
        display: flex;
        flex-wrap: wrap;
    }
    :global(.media) {
        margin-bottom: 10px;
    }

    .comentario .texto {
        white-space: pre-wrap;
        word-break: break-word;
        overflow: hidden;
        display: inline-block;
    }

    .respuestas {
        font-size: 0.7em;
        flex-wrap: wrap;
        display: flex;
        gap: 4px;
        flex-direction: row-reverse;
        justify-content: flex-end;
        grid-row: 2;
    }
    .respuestas-compactas {
        grid-row: 2;
        background: var(--color4);
        position: absolute;
        bottom: 0;
        right: 0;
        padding: 5px 10px;
        border-radius: 10px 0 0 0px;
        font-size: 12px;
        color: var(--color5);
    }

    .contenido .media {
        float: left;
        margin-right: 10px;
        max-width: 50%;
    }

    .color {
        width: 50px;
        height: 50px;
        background: orange;
        grid-area: color;
        display: flex;
        align-items: center;
        padding: 2px;
        /* text-align: center; */
        font-stretch: condensed;
        /* border-radius: 10; */
        justify-content: center;
        font-weight: 600;
        /* color: #822f0047; */
        color: #ffffffe3;
        border-radius: 4px;
    }
    /* Gorritos */
    /* .color-rojo::after, .color-multi::after, .comentarioMod .color::after, .color-navideño::after {
        content: '';
        background: url(/imagenes/colores/gorrito.png);
        position: absolute;
        top: 1px;
        left: 1px;
        height: 26px;
        width: 26px;
        background-size: 85%;
        background-repeat: no-repeat;
        transform: rotate(-4deg);
    } */

    .comentario .header {
        grid-template-areas: color;
        display: flex;
        align-items: center;
        margin-bottom: 0;
        font-size: 0.9em
    }

    .comentario .header span {
        margin-right: 10px;
    }

    .comentario .header .id {
        cursor: pointer;
    }

    .comentario .tag {
        background: #000000ad;
        padding: 0 5px;
        border-radius: 500px;
        display: flex;
        align-items: center;
    }

    .comentario .tiempo {
        margin-left: auto;
        opacity: 0.5;
        font-size: 12px;
    }

    .tag-op {
        background: var(--color5) !important;
    }
    .comentario:hover {
        background: var(--color4);
    }

    .resaltado {
        background: var(--color7)!important;
    }
    .eliminado {
        border-left: solid 2px red !important;
    }

    .color-rojo {background: #dd3226;}
    .color-verde {background: #53a538;}
    .color-amarillo {background: #ffc400;}
    .color-azul {background: #00408a;}
    .color-rosa {background: #ff74c1;}
    .color-negro {background: #000000;}
    .color-marron {background: #492916;}
    .color-white {color: #00abec;border-top: solid 4px #ffc400; background: white;}

    .color-rose-rubia {background: url(/imagenes/colores/rose-rubia.jpg); background-size: 100%; color:transparent;}
    .color-rose-azul {background: url(/imagenes/colores/rose-azul.jpg); background-size: 100%; color:transparent;}
    .color-rose-castaña {background: url(/imagenes/colores/rose-castaña.jpg); background-size: 100%; color:transparent;}
    .color-rose-violeta {background: url(/imagenes/colores/rose-violeta.jpg); background-size: 100%; color:transparent;}

    .color-multi {
        background: linear-gradient(#ffc400    25%, #00408a  25%, #00408a  50%, #53a538   50%, #53a538   75%, #dd3226  75%, #dd3226  100%);
        animation: multi .3s infinite;
    }

    .color-navideño {
        background: linear-gradient(#f0202e    33.3%, #ffffff  33.3%, #ffffff  66.3%, #008939   66.3%, #008939   100%) ;
        color: #ffc400;
        animation: navideño .4s infinite;
    }
    
    @keyframes navideño {
        33.3%  { background: linear-gradient(#008939    33.3%, #f0202e  33.3%, #f0202e  66.3%, #ffffff   66.3%, #ffffff   100%);}
        66.3%  { background: linear-gradient(#ffffff    33.3%, #008939  33.3%, #008939  66.3%, #f0202e   66.3%, #f0202e   100%);}
        100%   { background: linear-gradient(#f0202e    33.3%, #ffffff  33.3%, #ffffff  66.3%, #008939   66.3%, #008939   100%);}
    }
    @keyframes multi {
        20%  { background: linear-gradient(#dd3226 25%, #ffc400 25%, #ffc400 50%, #00408a 50%, #00408a 75%, #53a538 75%, #53a538 100%);}
        60%  { background: linear-gradient(#53a538 25%, #dd3226 25%, #dd3226 50%, #ffc400 50%, #ffc400 75%, #00408a 75%, #00408a 100%);}
        80%  { background: linear-gradient(#00408a 25%, #53a538 25%, #53a538 50%, #dd3226 50%, #dd3226 75%, #ffc400 75%, #ffc400 100%);}
        100% { background: linear-gradient(#ffc400 25%, #00408a 25%, #00408a 50%, #53a538 50%, #53a538 75%, #dd3226 75%, #dd3226 100%);}
    }

    .comentarioMod  {
        border-top: solid 2px;
        animation: borde-luz 0.3s infinite alternate-reverse;
        
    }
    
    .comentarioMod >.color {
        animation: luces 0.3s infinite alternate-reverse;
        color: white !important;
    }
    .comentarioAuxiliar  {
        border-top: solid 2px;
        animation: borde-serenito 0.3s infinite alternate-reverse;
        
    }
    .comentarioAuxiliar > .color{
        background: url(/imagenes/serenito.gif);
        background-size: 161px;
        color: transparent;
        background-position: center;
    }

    .nombreResaltado {
        color: var(--color6);
        font-weight: bold;
    }


    @keyframes borde-serenito {
        0% {border-color: rgb(255, 136, 0);}
        100% {border-color: transparent;}
    }
    @keyframes borde-luz {
        0% {border-color: red;}
        100% {border-color: blue;}
    }
    @keyframes luces {
        0% {
            background: red;
            border-color: red;

        }
        100% {
            background: blue;
            border-color: blue;

        }
    }

    .cptr {
        cursor: pointer;
    }
.dado {
    font-size: 2rem;
    font-family: 'euroFighter';
}

.idunico:hover {
   color: var(--color5)
}
@media (max-width: 600px) {
  .comentario :global(.restag), .respuestas-compactas {
      font-weight: bold !important;
  }
  .comentario :global(a) {font-weight: bold !important;}
}
    /* @media(max-width >600px) {} */

    /* .comentario-movil :glo.media {
  max-width: 100%;
  width: 100%;
}
.comentario-movil .color {
  height: 30px;
  position: relative;
  top: -8px;
  left: -8px;
}
.comentario-movil {
  grid-template-areas:
  "color header"
  "respuestas respuestas"
  "cuerpo cuerpo";
} */
</style>