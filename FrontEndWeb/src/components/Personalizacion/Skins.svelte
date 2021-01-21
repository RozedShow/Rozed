<script>
    import {Dialog, Button, Checkbox, ExpansionPanel, Ripple} from 'svelte-mui'
    import {localStore} from '../../localStore'
    import skinsStore from './skinsStore'

    let editandoSkin = false

    skinsStore.applicarEstilo()
    $: if($skinsStore) skinsStore.applicarEstilo()


</script>

<ul>
    {#each $skinsStore.skins as s }
        <li on:click={() => $skinsStore.activo = s.nombre} class:selecionado={$skinsStore.activo == s.nombre}>{s.nombre}
            <span style="margin-left:auto;">
                <Button icon dense title="Editar" on:click={() => editandoSkin = true}><span class="fe fe-edit"></span></Button> <Ripple/>
            </span>
        </li>
        {#if $skinsStore.activo == s.nombre}
            <Dialog width="500" bind:visible={editandoSkin} >
                <div slot="title">Ajustes</div>

                <textarea 
                    style=" background: var(--color3);" 
                    spellcheck="false"
                    bind:value={s.style}
                    placeholder="Ej {'h1 {color: red;}'} "
                    cols="30" rows="10"></textarea>
                <div slot="actions" class="actions center">
                    <Button color="primary" on:click={() => editandoSkin = false}>Ta bien</Button>
                </div>
            </Dialog>
        {/if}
    {/each}
</ul>
<Button color="primary" on:click={skinsStore.restaurarSkins}>Restaurar skins</Button>


<style>
      li {
        position: relative;
        cursor: pointer;
        height: 44px;
        -webkit-user-select: none;
        -moz-user-select: none;
        -ms-user-select: none;
        user-select: none;
        display: flex;
        align-items: center;
        padding: 0 16px;
        white-space: nowrap;
    }

    li:hover {
        background: rgba(255,255,255,.11);
    }
    .selecionado {
        border-right: 2px var(--color5) solid;
    }
</style>