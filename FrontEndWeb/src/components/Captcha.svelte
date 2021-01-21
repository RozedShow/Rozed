<script>
    import {onMount} from "svelte"
    import globalStore from "../globalStore"
    export let token
    export let visible = true

    if($globalStore.usuario.estaAutenticado && $globalStore.usuario.esMod)
        visible = false

    onMount(() => {
        if(window.hcaptcha) cargarCaptcha()
        else setTimeout(cargarCaptcha, 500)
    })

    function cargarCaptcha() {
        window.hcaptcha.render("super-captcha", {
            "theme": "dark",
            "sitekey": window.config.hCaptchaSiteKey
        })
    }

    window.onCaptcha = (e) => {
        token = e
    }
</script>

{#if visible}
    <div class="h-captcha"
    id="super-captcha"
    style="display:flex" 
    data-callback="onCaptcha" 
    data-theme="dark" 
    data-sitekey="{window.config.hCaptchaSiteKey}"></div>
    
{/if}

<style>
    .h-captcha > :global(*) {
        margin: auto
    }
</style>