<script>
    import Media from '../Media.svelte'
    import {fade} from 'svelte/transition'
    export let comentarios = []
    export let visible = true;

    function onMediaClick(e) {
        visible = false
    }

</script>

{#if visible}
    <div transition:fade={{duration:150}} class="fondo" style="z-index:20" on:click={() => visible = false}>
        <section class="carpeta-media panel">
            <h3>Archivos del roz</h3>
            <ul>
                {#each comentarios as c}
                    {#if c.media}
                        <li>
                            <Media media={c.media} modoCuadrado={true} />
                            <a
                                on:click={onMediaClick}
                                href="#{c.id}" class="click-area">
                                
                            </a>
                        </li>
                    {/if}
                {/each}
            </ul>
        </section>
    </div>
{/if}
<style>
    .carpeta-media {
        background: red;
    }
    .fondo {
        position: fixed;
        height: 100vh;
        width: 100vw;
        top:0;
        left:0;
        display: flex;
        justify-content: center;
        background: #0000004a;
    }

    ul {
        display: flex;
        gap: 10px;
        flex-wrap: wrap;
        justify-content: center;
    }
    ul  li {
        width: 128px;
        height: 128px;
        position: relative;
        transition: 0.1s;
    }

    .click-area {
        position: absolute;
        width: 100%;
        height: 100%;
        top:0;
        left:0;
    }

    section {
        max-height: 80vh;
        max-width: 80vw;
        align-self: center;
        box-shadow: #00000075 0 8px 10px;
        border-top: 2px solid var(--color5);
        overflow: scroll;
    }

    h3 {
        text-align: center;
        margin-bottom: 10px;
    }

    .carpeta-media :global(.medialink){
        display: none !important;
    }

    li:hover {
        transform: translateY(-10px);
    }
    @media(max-width:600px) {
        section {
            width: 100vw;
            max-width: 100vw;
        }
    }
</style>