<script>
    import { abrir } from "../Dialogos/Dialogos.svelte";
    import { Button, Checkbox } from "svelte-mui";
    import BarraModeracion from "../Moderacion/BarraModeracion.svelte";
    import Media from "../Media.svelte";
    import RChanClient from "../../RChanClient";

    let medias = window.model.medias;
    // let comentarios = window.model.comentarios

    medias.forEach((m) => (m.seleccionado = false));
    medias = medias;

    $: selecionados = medias.filter(m => m.seleccionado);

    let eliminando = false
    
    async function eliminarMedias() {
        try {
            eliminando = true
            await RChanClient.eliminarMedias(selecionados.map(s => s.id))
            medias = medias.filter(m =>!selecionados.map(s => s.id).includes(m.id))
            selecionados = []
            
        } catch (error) {
            
        }
        eliminando = false
    }
</script>


<BarraModeracion />
<section class="media-home">
    
    {#if selecionados.length > 0}
        <div style="justify-content: center;display: flex; margin:8px">
            <Button on:click={eliminarMedias} 
                color="var(--color5)"
                disabled = {eliminando}
                raised>Eliminar {selecionados.length} archivos</Button>
        </div>
    {/if}
    <ul>
        {#each medias as m}
            <li>
                <Media media={m} modoCuadrado={true} />
                <!-- <Button color="var(--color1)" raised on:click={() => abrir.eliminarMedia(m.id)} style="display:none">Eliminar</Button> -->
                <div class="check">
                    <Checkbox bind:checked={m.seleccionado} />
                </div>
            </li>
        {/each}
    </ul>
</section>

<style>
    .check {
        position: absolute;
        top: 0;
        left: 0;
        border-radius: 0 0 50% 0;
        background: #0000008a;
    }
    ul {
        display: flex;
        gap: 4px;
        flex-wrap: wrap;
    }
    ul > li {
        width: 128px;
        height: 128px;
        position: relative;
    }

    li :global(button) {
        position: absolute;
        bottom: 0;
    }

    li:hover :global(button) {
        display: block !important;
        right: 10px;
    }

    .media-home :global(.media) {
        max-width: inherit;
        width: 100%;
        height: 100%;
    }
</style>