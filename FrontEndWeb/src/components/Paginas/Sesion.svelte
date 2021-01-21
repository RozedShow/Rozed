<script>
    import {Textfield, Button, Ripple} from "svelte-mui"
    import RChanClient from "../../RChanClient";
    import Captcha from "../Captcha.svelte";
    import ErrorValidacion from "../ErrorValidacion.svelte";
    import config from "../../config"

    let username = ""
    let password = ""
    let captcha = ""
    let error = null
    let codigo =  ""
    if(window.model && window.model.codigoDeInvitacion) {
        codigo = window.model.codigoDeInvitacion
    }

    export let modo = "login"

    $: modoRegistro = modo == "registro"

    async function accion(e) {
        console.log(captcha);
        try {
            if(modoRegistro)
                await RChanClient.registrase(username, password, captcha, codigo)
            else {
                let res = await RChanClient.logearse(username, password)
                return;

            }
        } catch (e) {
            console.log(e);
            error = e.response.data
            return
        }
        window.location = "/"
        // location.reload();
    }


</script>
<div class="fondo"></div>
<main>
    <!-- <video src="623eb91fd792a152481ebe7cecc2ce9f.mp4" loop autoplay muted></video> -->

    <section >
        {#if !config.general.registroAbierto && !codigo &&  modoRegistro}
        <h1>Hola anon</h1>
        <h1>El registro esta desactivado</h1>
        {:else if !modoRegistro}
            <h2>Para crear y responder rozes en Rozed debes iniciar una sesion</h2>
            <h3>No tenes cuenta? Enfermo!, <a on:click="{()=> modoRegistro=true}" href="#Registro"style="color:var(--color5) ">Registrate ahora mismo aca</a> </h3>
        {:else}
            <h2>Para crear y responder rozes en Rozed debes iniciar una sesion</h2>
            <h2>Registrate con cofianza</h2>
            <h4>Tu ip esta a salvo, desde ya que si</h4>
        {/if}
        <ErrorValidacion {error}/>
        <form on:submit|preventDefault={accion}>
            <Textfield
            autocomplete="off"
            label="Nombre de usuario"
            required
            bind:value={username}
            message="kikefoster4000"
            />
            <Textfield
                autocomplete="off"
                label="Contraseña"
                type="password"
                required
                bind:value={password}
                message="aynose1234"
            />
            {#if modoRegistro}
                <Captcha visible={config.general.captchaRegistro}  bind:token={captcha}/>
             {/if}
            <div style="display:flex; justify-content: center;">
                <Button >{modoRegistro?"Registrarse":"Entrar"}</Button>
            </div>


        </form>
    </section>
</main>

<style>
    main {
        margin:auto;
        height: auto;
        padding: 16px;
        max-width: 1600px;
        margin-top: 80px;
        overflow: hidden;
    }

    section {
        max-width: 600px;
        display: flex;
        flex-direction: column;
        gap: 16px;
        background: var(--color2);
        padding: 16px ;
        border-radius: 4px;
        border-top: solid 2px var(--color5);
    }
    form {
        color: white !important;
        /* background: var(--color2); */
    }

    video {
        position: fixed;
        z-index: -1;
        top: 50%;
        left: 50%;
        min-width: 100%;
        min-height: calc(100vh - 400px);
        width: auto;
        transform: translateX(-50%) translateY(-50%);
        opacity: 0.4;
        /* filter: contrast(1.5) brightness(1.5); */
    }

    .fondo{
        position: fixed;
        top:0;
        left:0;
        width: 100vw;
        height: 100vh;
        background: var(--color1);
        z-index: -100;
    }

    :global(.label) {
        color: #ffffffcc !important
    }
</style>