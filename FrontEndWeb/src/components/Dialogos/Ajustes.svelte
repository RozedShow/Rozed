<script>
    import {Dialog, Button, Checkbox, ExpansionPanel, Ripple} from 'svelte-mui'
    import {localStore} from '../../localStore'
    import Skins from '../Personalizacion/Skins.svelte'
    import ajustesConfigStore from './ajustesConfigStore'

    export let visible = true

    $: if($ajustesConfigStore) actualizarConfiguracion()

    setTimeout(actualizarConfiguracion, 1)
    function actualizarConfiguracion() {
        let css = `
            #fondo-global {
                ${$ajustesConfigStore.usarImagen?`background-image: url(${$ajustesConfigStore.imagen})`: `background:${$ajustesConfigStore.colorFondo}`};
                background-size:${$ajustesConfigStore.modoCover?'cover':'auto'} ;
            }
        `
        if(!$ajustesConfigStore.fondoAburrido)
        {
            css = `
            #fondo-global {
                background-image: url(/imagenes/rosed.png) ;
                background-size:auto ;
            }
        `
        }
        let style = window.document.styleSheets[0];
        style.insertRule(css, style.cssRules.length)
        
        if($ajustesConfigStore.scrollAncho) {
            style.insertRule(`
            ::-webkit-scrollbar {
                width: 10px !important;
            }`,style.cssRules.length)
        }
    }

    function actualizarYCerrar() {
        actualizarConfiguracion()
        visible = false
    }

    let group = '';

</script>
<div class="ajustes">
    <Dialog  width="500" bind:visible={visible}>
        <div slot="title">Ajustes</div>
        <ExpansionPanel bind:group name="Personalizacion">
            <Checkbox  bind:checked={$ajustesConfigStore.scrollAncho} right>Scroll ancho</Checkbox>  
            <Checkbox  bind:checked={$ajustesConfigStore.tagClasico} right>Tag clasico</Checkbox>  
            <Checkbox  bind:checked={$ajustesConfigStore.fondoAburrido} right>Fondo personalizado</Checkbox>
            {#if $ajustesConfigStore.fondoAburrido}
                <Checkbox  bind:checked={$ajustesConfigStore.usarImagen} right>Usar imagen</Checkbox>
                {/if}
                {#if $ajustesConfigStore.fondoAburrido && !$ajustesConfigStore.usarImagen}
                <div style="display:flex"> 
                    <label  for="color">Color:</label>  
                    <input bind:value={$ajustesConfigStore.colorFondo}  class="colorpicker" type="color" name="color">
                </div>
                {/if}
                {#if $ajustesConfigStore.fondoAburrido && $ajustesConfigStore.usarImagen}
                <div style="display:flex;align-items: baseline;gap: 10px;">
                    <label  for="imagen">Imagen:</label>  
                    <input style="background: var(--color4);" bind:value={$ajustesConfigStore.imagen}  type="text" name="imagen">
                </div>
                <Checkbox  bind:checked={$ajustesConfigStore.modoCover} right>Modo Cover</Checkbox>
            {/if}
        </ExpansionPanel>
    
        <ExpansionPanel bind:group name="Auto censura">
            <textarea 
                style=" background: var(--color3);" 
                spellcheck="false"
                bind:value={$ajustesConfigStore.palabrasHideadas}
                placeholder="Podes usar palabras y frases(palabras separadas guion bajo en vez de espacios). Ej sidoca huele tengo_un_video minubi insta se_le_da, etc"
                cols="30" rows="10"></textarea>
        </ExpansionPanel>
        <ExpansionPanel bind:group name="Skins">
           <Skins/>
        </ExpansionPanel>
        
        <div slot="actions" class="actions center">
            <Button color="primary" on:click={actualizarYCerrar}>Lito</Button>
        </div>
    </Dialog>
</div>


<style>
    .colorpicker {
        height: 25px;
        width: 23px;
        padding: 0;
        margin-left: auto;
        margin-right: 8px;
    }
    .ajustes :global( .content .panel) {
        padding: 0;
        box-shadow: none !important;
        background: #0a10176b !important;
    }

    .ajustes :global(.content ) {
        padding: 0px 8px;
    }
</style>