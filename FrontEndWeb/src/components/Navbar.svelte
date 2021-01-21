<script>
    import {Ripple, Button} from 'svelte-mui'
    import config, {configStore} from '../config'
    import FormularioHilo from './Hilos/FormularioHilo.svelte'
    import Notificaciones from './Notificaciones.svelte'
    import MenuPrincipal from './MenuPrincipal.svelte'
    import FormularioLogin from './FormularioLogin.svelte'
    import globalStore from '../globalStore'
    import MensajeRotativo from './MensajeRotativo.svelte'
    import Dialogos from './Dialogos/Dialogos.svelte'
    import DenunciasNav from './Moderacion/DenunciasNav.svelte'
    import Signal from '../signal'
    import SelectorDeComentarios from './Moderacion/SelectorDeComentarios.svelte';
    import Subir from './Subir.svelte'
    import { onMount } from 'svelte';
    import NavCategorias from './NavCategorias.svelte'
    import NadieLoSabra from './Especiales/NadieLoSabra.svelte'
    import Estadisticas from './Estadisticas.svelte';
    // import Lucesitas from './Lucesitas.svelte'
    // import Cuetitos from './Especiales/Cuetitos.svelte'
    // import CuentaRegresiva from './Especiales/CuentaRegresiva.svelte';
    

    export let notificaciones = window.notificaciones || []

    let mostrarMenu = false
    let mostrarFormularioHilo = false
    let computadorasConectadas = window.estadisticas.computadorasConectadas

    let  protocoloMessi = false
    $: protocoloMessi =  $configStore.general.modoMessi 
    let  protocoloSerenito = false
    $: protocoloSerenito =  $configStore.general.modoSerenito 

    let mostrarComputadorasConectadas = false

    let oculta
    let ocultarCategorias = false
    let compacta = false
    let prevScrollpos = window.pageYOffset;

    function onScroll(e) {
        compacta = !(window.pageYOffset == 0)
        ocultarCategorias = compacta
    
        let currentScrollPos = window.pageYOffset;
        oculta = currentScrollPos > prevScrollpos;
        prevScrollpos = currentScrollPos;
    }

    let height = 0
    
    // $: if(height) {
    //     try {
    //         document.querySelector("main").style.marginTop = (height - 17) + "px"
    //     } catch (error) {console.log(error)}
    // }

    let style = window.document.styleSheets[0];
    $: if(protocoloMessi) {
        style.insertRule("body {--color5:rgb(28 185 208)!important}", style.cssRules.length)
    } else if(protocoloSerenito) {
        style.insertRule("body {--color5:rgb(255 124 36)!important}", style.cssRules.length)
    }

    $: if(mostrarFormularioHilo && !$globalStore.usuario.estaAutenticado) {
        window.location = '/Inicio'
    }
    
    let scrollY

    let version = `Union de religiones - Version Chad ${window.config.general.version}`
    if(protocoloMessi) version = `Protocolo Messi activado - Version Chad ${window.config.general.version}`
    else if(protocoloSerenito) version = `Protocolo Serenito activado - Version Chad ${window.config.general.version}`
</script>

<svelte:window  on:scroll={onScroll} bind:scrollY={scrollY}/>

<NadieLoSabra/>
<header
    class:oculta
    class:protocoloMessi
    class:protocoloSerenito
    bind:offsetHeight={height}
>
    <nav>
        <!-- <Lucesitas/> -->
        <div class="nav-principal" class:modoSticky={scrollY > 200}>
            <div class="fondo" style="position:absolute;left:0;top:0;width:100%">
                <!-- <Cuetitos/> -->
                <!-- <CuentaRegresiva/> -->
                <Estadisticas/>
            </div>
            <span on:click={() => mostrarMenu = !mostrarMenu} style="padding: 0 8px;">
                <icon class="fe fe-menu"/>
                <Ripple/>
            </span>
            <a href="/" style="font-family: euroFighter">
                <h3 class="rozed">ROZED <span class="version">{version} </span>
                </h3>
                <Ripple/>
            </a>
            <!-- <CuentaRegresiva/> -->
            <!-- <MensajeRotativo/> -->
            <div class="estadisticas">
                <!-- {#if mostrarComputadorasConectadas}
                    <span transition:fade={{duration: 5000 }}   on:introend="{() => mostrarComputadorasConectadas = false}">
                        {computadorasConectadas} computadora{computadorasConectadas!=1?'s':''} conectada{computadorasConectadas!=1?'s':''}
                    </span>
                {/if} -->
            </div>
            {#if protocoloMessi}
                <div class="messi"></div>
            {:else if protocoloSerenito}
                <!-- <div class="serenito"></div> -->
            {/if}

            <div class="nav-botones" style="position: relative;">
                
                {#if $globalStore.usuario.esMod || $globalStore.usuario.esAuxiliar}
                    <a href="/Moderacion">
                        <!-- <span style="height: 48px;display: flex;align-items: center;"> -->
                            <!-- <icon class="fe fe-triangle"/>
                            -->
                            <!-- <Ripple/> -->
                        <!-- </span> -->
                        <Button icon dense>
                            <span style="top: -1px;
                            font-size: 24px;
                            padding: 0 4px;">✡</span>
                        </Button>
                    </a>
                {/if}
                {#if $globalStore.usuario.esMod || $globalStore.usuario.esAuxiliar}
                    <DenunciasNav/>
                {/if}

                <a href="/Buscar"class="nav-boton"  style="height:100%">
                    <!-- <Ripple/> -->
                    <Button icon dense>
                        <span class="fe fe-search"></span>
                    </Button>
                </a>
                {#if $globalStore.usuario.estaAutenticado}
                    <Notificaciones bind:notificaciones/>
                {:else}
                    <a href="/Login"class="nav-boton"  style="height:100%">
                        <!-- <Ripple/> -->
                        <Button icon dense>
                            <span class="fe fe-user"></span>
                        </Button>
                    </a>
                {/if}

            </div>
            {#if !protocoloMessi || $globalStore.usuario.esMod}
                <span class="nav-boton crear-hilo-boton" on:click={() => mostrarFormularioHilo = true}>
                    <span style="width:max-content; margin-right: 6px;cursor: pointer;">Roz</span>
                    <span class="fe fe-plus-square"></span>
                    <Ripple/>
                </span>
            {/if}

            <FormularioHilo bind:mostrar ={mostrarFormularioHilo}/>
            <FormularioLogin/>
        </nav>
       <NavCategorias visible={!ocultarCategorias}></NavCategorias>
</header>
<MenuPrincipal bind:mostrar={mostrarMenu}/>


<Dialogos></Dialogos>

{#if $globalStore.usuario.estaAutenticado && $globalStore.usuario.esMod}
    <SelectorDeComentarios/>
{/if}
<Subir></Subir>

<!-- <Personalizacion></Personalizacion> -->
<style>
    /*NAVBAR*/
.protocoloMessi  .nav-principal{
    background: rgb(0, 133, 241) !important;
}
.protocoloSerenito  .nav-principal{
    background-image: url(/imagenes/serenito.gif) !important;
    background-size: 98px !important;
    background-position-y: center !important;
}
.protocoloSerenito :global(.noti-cont) {
    background: #5f05fc !important;
}

.nav-principal {
    border-top: solid var(--color5) 2px;
    align-items: stretch !important;
    height: 48px;
    /* margin-bottom: 10px; */
}
.nav-principal .fondo {
    height: 48px;
}
.nav-principal>*, .nav-principal nav-botones  span{
    /* height: 48px; */
    display: flex;
    align-items: center;
    /* margin-bottom: 10px; */
}

.nav-principal, .nav-principal a, :global(.nav-boton) {
    display: flex;
    align-items: center;
}
.nav-principal > {
    flex: 1;
}

/* :global(.nav-boton), .nav-principal a, .nav-principal icon {
    padding: 0 8px;
} */

.nav-botones {
  margin-left: auto;
  align-self: center;
  font-size: 16px;
  display: flex;
  margin-right: 16px;
}
.nav-botones :global(button) {
    overflow: visible;
}
.crear-hilo-boton{
    height: 100%;
    background: var(--color5);
    font-family: 'euroFighter';
    font-size: 18px;
    padding: 8px;
    color: var(--color-texto1);
}
:global(.nav-boton) {
  color: var(--color-texto1);
  display: inline-flex;
  position: relative;
}

header {
    /* margin-bottom: 10px; */
    /* position: fixed; */
    z-index: 10;
    top: 0;
    width: 100vw;
    transition: linear 0.2s;
    position: relative;
}
.compacta  .nav-categorias {
    margin: 0;
    padding: 0;
    gap: 4px;
}
/* .compacta  .nav-categorias a{
    font-size: 14px !important;
} */

/* .nav-categorias {
    opacity: 1;
    transition: linear 0.1s;

} */

/* :global(main) {
    margin-top: 120px;
} */

/* .oculta {
    transform: translateY(-120px) ;
} */

/* .ocultarCategorias {
    opacity: 0;
} */

@media(max-width:600px)
{
    .nav-categorias {
        display: none;
    }
    header {
        margin-bottom: 10px;
        position: fixed;
        z-index: 10;
        top: 0;
        width: 100vw;
        transition: linear 0.2s;
    }
    :global(main){
        margin-top: 50px;
    }

    .oculta {
        transform: translateY(-50px) ;
    }
}

.version {
    font-size: 10px;
    position: absolute;
    bottom: 1px;
    left:4px;
    font-family: sans-serif;
    width: max-content;
}

:global(.noti-cont) {
    position: absolute;
    left: 12px;
    width: auto;
    top: 2px;
    font-family: helvetica;
}
.nav-principal :global(.noti-cont span)  {
    position: relative;
}

.estadisticas {
    text-transform: uppercase;
    font-size: 12px;
    text-align: center;
    color: #334d67;
    justify-self: center;
    margin: auto;
    overflow: hidden;
}
.messi {
    background-image: url("/imagenes/messi.gif");
    height: 46px;
    max-height: 100%;
    background-size: 70px;
    position: absolute;
    mix-blend-mode: overlay;
    width: 100%;
    pointer-events: none; 
}
/* Gorritos */
/* .rozed::after {
    content: "";
    position: absolute;
    top: 5px;
    right: 7px;
    height: 32px;
    width: 32px;
    transform: rotate(-4deg) scaleX(-1);
    background: url(/imagenes/colores/gorrito.png) 0% 0% / 85% no-repeat;
    background-repeat: no-repeat;
} */

.modoSticky {
    position: fixed;
    top: 0;
    width: 100vw;
    height: 36px;
    z-index: 1;
}

.modoSticky .rozed::after {
    top: 0px;
}
.modoSticky .version {
    display: none;
}
.modoSticky .fondo {
    height: 36px;
}
.luto {
    background: url(/imagenes/luto.png);
    width: 16px;
    height: 50px;
    background-size: 100%;
    position: absolute;
    top: 11px;
    background-repeat: no-repeat;
    left: 78px;
}
/* .estadisticas span {
    animation: baja 5s infinite linear;
}

@keyframes baja {
    0% {
        transform: translateY(-40px);
    }
    100% {
        transform: translateY(40px);
    }
} */
</style>
