<script>
    import { Button } from "svelte-mui"
    import HiloList from "../Hilos/HiloList.svelte"
    import RChanClient from "../../RChanClient";
    import Spinner from "../Spinner.svelte";

    let cadenaDeBusqueda = ""

    let hiloList = {
        hilos: []
    } 
    let cargando = false
    let sinResultados = false

    async function buscar() {
        try {
            sinResultados = false
            let cargando = true
            let res = await RChanClient.buscar(cadenaDeBusqueda)
            sinResultados = res.data.length == 0
            hiloList.hilos = res.data
        } catch (e){
            console.log(e);
        }
        let cargando = false
    }

    
</script>

<main>
    <h3 style="text-align:center; margin:16px 10px">Buscar Roz</h3>
    
    <section>
        <form on:submit|preventDefault={buscar} class="panel input-busqueda">
            <!-- svelte-ignore a11y-autofocus -->
            <input
                autocomplete="off"
                placeholder="Alguna palabra en el titulo del roz"
                autofocus
                bind:value={cadenaDeBusqueda}
            />
            <Spinner {cargando}>
                <Button on:click={buscar} raised color="var(--color5)">Buscar</Button>
            </Spinner>
        </form>
    
    </section>
    {#if sinResultados}
        <h3 style="text-align:center">No se encontraron rozs</h3>
    {/if}

    <HiloList bind:hiloList={hiloList}></HiloList>

</main>

<style>

    section {
        display: flex;
        justify-content: center;
        align-items: center;
    }
    .input-busqueda {
        gap:10px;
        border-top: solid 2px var(--color5);
        max-width: 800px;
        flex:1;
        display: flex;
        margin-bottom: 10px;
    }

    @media(max-width: 600px) {
        main {
            margin-top: 60px;
        }
    }
</style>