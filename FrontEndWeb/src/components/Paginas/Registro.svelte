<script>
    import {Textfield, Button, Ripple, Checkbox} from "svelte-mui"
    import RChanClient from "../../RChanClient";
    import Captcha from "../Captcha.svelte";
    import ErrorValidacion from "../ErrorValidacion.svelte";
    import config from "../../config"

    let username = ""
    let password = ""
    let terminos = false
    let captcha = ""
    let error = null
    let codigo =  ""
    if(window.model && window.model.codigoDeInvitacion) {
        codigo = window.model.codigoDeInvitacion
    }

    async function accion(e) {
        console.log(captcha);
        try {
            await RChanClient.registrase(username, password, captcha, codigo)
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
        {#if config.general.registroAbierto || codigo}
            <h2>Registrate con cofianza</h2>
            <h4>Tu ip esta a salvo, desde ya que si</h4>
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
                    message="Si te la olvidas, domado"
                />
                <a style="color:var(--color5); text-align:center; display:block;font-weight: bold;font-size: 19px;" target="_blanck" href="/reglas.html">Ver reglas</a>
                <Checkbox right bind:checked={terminos}><div style="white-space: normal; text-align: center;">Yo Anon juro solemnemente seguir las reglas de Rozed </div></Checkbox>
                <Captcha visible={config.general.captchaRegistro}  bind:token={captcha}/>

                <div style="display:flex; justify-content: center; margin-top: 8px">
                    <Button disabled={!terminos}>Registrarse</Button>
                </div>

            </form>
        {:else}
            <h2>Lo siento anon, el registro esta temporalmente deshabilitado</h2>
        {/if}
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
        z-index: -100;
        background: url('/imagenes/rosed.png');
        background-size: cover !important;
    }
    :global(.label) {
        color: #ffffffcc !important
    }
</style>