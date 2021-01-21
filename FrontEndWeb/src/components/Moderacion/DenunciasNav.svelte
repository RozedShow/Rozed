<script>
    import { Ripple, Button } from "svelte-mui";
    import { EstadoDenuncia } from "../../enums";
    import globalStore from "../../globalStore";
    import Signal from "../../signal";
    import Denuncia from "../Denuncia.svelte";

    let denuncias = window.denuncias || [];
    $: denunciasActivas = denuncias.filter(d => d.estado == EstadoDenuncia.NoRevisada)

    let mostrar = false;

    $: if(denuncias.filter(d => d.estado == EstadoDenuncia.NoRevisada).length == 0) {
        mostrar = false
    }

    if($globalStore.usuario.esMod || $globalStore.usuario.esAuxiliar) {
        Signal.subscribirAModeracion();
    }

    const underAttack = new Audio("/audio/underAttack.mp3")
    const toing = new Audio("/audio/toing.mp3")

    Signal.coneccion.on("nuevaDenuncia", (denuncia) => {
        underAttack.play()

        denuncias = [denuncia, ...denuncias]
        mostrar = true;
    })

    Signal.coneccion.on("denunciasRechazadas", (ids) => {
        if(ids.length == 0) return
        toing.play()

        denuncias = denuncias.map(d => {
            if(ids.includes(d.id)) 
                d.estado = EstadoDenuncia.Rechazada
            return d
        })
    })

    Signal.coneccion.on("denunciasAceptadas", (ids) => {
        if(ids.length == 0) return
        toing.play()

        denuncias = denuncias.map(d => {
            if(ids.includes(d.id)) 
                d.estado = EstadoDenuncia.Aceptada
            return d
        })
    })
</script>


<svelte:head>
    <title>{denunciasActivas.length != 0?`{${denunciasActivas.length}}!`:''} {document.title.split("!").pop()}</title>
</svelte:head>
{#if $globalStore.usuario.esMod || $globalStore.usuario.esAuxiliar}

    <Button icon dense on:click={() => mostrar = !mostrar } style="position:relative">
        <!-- <Ripple /> -->
        <span class="fe fe-alert-circle" />
        {#if denuncias.filter(d => d.estado == EstadoDenuncia.NoRevisada).length != 0}
            <div class="noti-cont" style="position: absolute;">
                <span>{denuncias.filter(d => d.estado == EstadoDenuncia.NoRevisada).length}</span>
            </div>
        {/if}
    </Button>

    {#if mostrar}
        <div class="denuncias-nav">
            <ul>
                {#each denuncias as d (d.id)}
                    <Denuncia bind:denuncia={d} />
                {:else}
                    <h3 style="text-align:center">No hay denuncias</h3>
                {/each}
            </ul>
        </div>
    {/if}

{/if}

<style>
    .denuncias-nav {
        position: absolute;
        right: -29px !important;
        top: 51px;
        left: auto;
        padding: 4px;
        min-width: 400px;
        width: fit-content;
        z-index: 10;
        background: #9c1010;
        font-size: 0.7em;
        max-height: 90vh;
        overflow-x: scroll;
    }
    ul :global(.hilo) 
    {
        width: 100%;
        height: 100px !important
    }
    ul :global(.hilo img) 
    {
        height: fit-content;
    }

    @media(max-width:600px) {
        .denuncias-nav {
            left: 8px;
            right: 0 !important;
            max-width: calc(100vw - 16px);
            width: 100vh;
            min-width: initial;
            position: fixed;
        }
    }
</style>