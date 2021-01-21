<script>
    import {Textfield, Button, Ripple, Checkbox} from "svelte-mui"
    import RChanClient from "../../RChanClient";
    import Captcha from "../Captcha.svelte";
    import ErrorValidacion from "../ErrorValidacion.svelte";
    import config from "../../config"

    let username = ""
    let password = ""
    let captcha = ""
    let error = null
    let usarToken = false
    let token = ""

    async function accion(e) {
        console.log(captcha);
        try {
            let res = null
            if(!usarToken) {
                res = await RChanClient.logearse(username, password)
            } else {
                res = await RChanClient.restaurarSesion(token)
            }
            // if(res.data.redirect) {
            //     window.location.href = response.data.redirect
            // }
        } catch (e) {
            console.log(e);
            error = e.response.data
            return
        }
        console.log("chan")
        window.location = "/"
        // location.reload();
    }


</script>
<div class="fondo"></div>

<main>
    <!-- <video src="623eb91fd792a152481ebe7cecc2ce9f.mp4" loop autoplay muted></video> -->

    <section >
         <ErrorValidacion {error}/>
        <h1>Hola anon!</h1>
        <h2>Para crear y responder rozes en Rozed debes iniciar una sesion</h2>
        <h3>No tenes un token o una cuenta? Enfermo!,  podes <a href="/Inicio"style="color:var(--color5) ">Iniciar Sesion</a> o <a href="/Registro"style="color:var(--color5) ">registrate</a> </h3>

        
        <form on:submit|preventDefault={accion}>
            <Checkbox bind:checked={usarToken}>Usar Token</Checkbox>
            {#if !usarToken}
            <ErrorValidacion {error}/>
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
                {:else}
                    <Textfield
                    autocomplete="off"
                    label="Token de sesion"
                    bind:value={token}
                    />
                {/if}

                <div style="display:flex; justify-content: center;">
                    <Button >Entrar</Button>
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
        z-index: -100;
        background: url('/imagenes/rosed.png');
        background-size: cover !important;
    }

    :global(.label) {
        color: #ffffffcc !important
    }
</style>