<script>
    import config from '../config'
    import { Ripple, Dialog, Button } from 'svelte-mui'
    import { localStore } from '../localStore'
    import Cookie from 'js-cookie'
    export let visible = true

    let localConfig = localStore("NavCategorias", {oculta: true, favoritas: [1, 3, 6, 10]})
    if(!$localConfig.favoritas) $localConfig.favoritas = []
    
    function toggle() {
        $localConfig.oculta = !$localConfig.oculta
    }
    $: oculta = $localConfig.oculta

    $: favoritas = $localConfig.favoritas
    $: favoritas && Cookie.set('categoriasFavoritas', favoritas, { expires: 696969 })

    let configurando = false


</script>

<nav class="nav-categorias"
    class:visible
    class:oculta>
    <div class="colapsar-categorias categoria favorita" on:click={toggle}>
        <div class="fe fe-circle"></div>
        <Ripple color="var(--color5)"/>
    </div>
    <a href="/Favoritas" title="favoritas" class="colapsar-categorias favorita categoria" style="display: flex;align-items: center;"><span class="fe fe-star"></span></a>
    <div title="Configurar favoritas" class="colapsar-categorias fe fe-settings categoria favorita cpt" on:click={() => configurando = true}></div>

    {#each config.categorias.filter(c => favoritas.includes(c.id)) as c (c.id)}
        <a class="categoria favorita"href="/{c.nombreCorto}" title={c.nombre}>{c.nombreCorto}</a>
    {/each}
    <span class="categoria sep favorita" style="border-radius: 0 20px 20px 0;color:transparent; margin-right:4px">.</span>
<!-- 
    <a class="categoria comunacha" href="/GOLD" title="GOLD"
        style="border-radius: 20px; background: linear-gradient(45deg, #f59e00, #ffd383); margin-right: 4px; color: #7b5500;">GOLD </a> -->
    <span class="categoria sep" style="border-radius: 20px 0 0 20px;color:transparent">.</span>
    {#each config.categorias.filter(c => !favoritas.includes(c.id)) as c (c.id)}
        <a class="categoria comunacha" href="/{c.nombreCorto}" title={c.nombre}>{c.nombreCorto} </a>
    {/each}
    <span class="categoria sep" style="border-radius: 0 20px 20px 0;color:transparent">.</span>
</nav>

{#if configurando} 
    <Dialog width="600" bind:visible={visible}>
        <div slot="title">Categorias favoritas</div>
        <slot name="body">
            <span style="color:var(--color5)">Favoritas ({favoritas.length}):</span>
            <div class="favoritas container-categorias cpt">
                {#each config.categorias.filter(c => favoritas.includes(c.id)) as c (c.id)}
                <span class="categoria favorita" href="/{c.nombreCorto}" on:click={() => $localConfig.favoritas = $localConfig.favoritas.filter(c1 => c1 != c.id )} title={c.nombre}>{c.nombreCorto}</span>
                {/each}
            </div>
            
            <span style="color:var(--color5)">Comunachas ({ config.categorias.length - favoritas.length}):</span>
            <div class="no-favoritas container-categorias cpt" style="margin-top:10px">
                {#each config.categorias.filter(c => !favoritas.includes(c.id)) as c (c.id)}
                    <span class="categoria" on:click={() => $localConfig.favoritas = [...$localConfig.favoritas, c.id]} title={c.nombre}>{c.nombreCorto}</span>
                {/each}
            </div>
        </slot>
        
        <div slot="actions" class="actions center">
            <Button color="primary" on:click={() => configurando = false}>Ok</Button>
        </div>
    </Dialog>
{/if}

<style>
    .nav-categorias {
        display: flex;
        flex-wrap: wrap;
        margin-bottom: 8px;
        justify-content: center;
        margin-top: 10px;
        gap: 4px 0;
        transition: 0.2s;
    }

    .categoria {
        padding: 2px 6px !important;
        grid-column: 1/21;
        padding: 2px 8px;
        font-size: 14px;
        color: var(--color-texto1);
        display: flex;
        align-items: center;
        height: fit-content;
        align-items: center;
        font-stretch: condensed;
        background: var(--color5);
        transition: 0.2s;
        user-select: none;
    }
    .categoria:first-child {
        border-radius: 20px 0 0 20px;
    }
    .categoria:last-child, :not(.favorita)+ .favorita .comunacha:last-of-type {
        border-radius: 0 20px 20px 0;
    }

    .colapsar-categorias {
        font-size: 16px;
        transition: 0.2s;
    }

    .oculta a, .oculta .sep {
        display: none;
        font-size: 10px !important;
    }

    .oculta {
        margin: 0;
        justify-content: left;
    }
    .oculta .colapsar-categorias {
        /* border-radius: 0 20px 20px 0; */
        border-radius: 0 0 11px 0px;
        background: var(--color5);
        font-size: 10px;
    }
    .oculta .colapsar-categorias:nth-child(1) {
        border-radius: 0 0 0 11px;
    }
    .oculta .colapsar-categorias:nth-child(2) {
        border-radius: 0;
    }
    .oculta .colapsar-categorias:nth-child(3) {
        border-radius: 0 0 11px 0;
    }
    @media(max-width:600px) {
        .nav-categorias {display: none}
    }

    .container-categorias {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
    }
    .favorita {
        background:  var(--color7);
    }
</style>