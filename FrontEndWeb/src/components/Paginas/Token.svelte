<script>
    import {Textfield, Button, Ripple, Checkbox} from "svelte-mui"
    import RChanClient from "../../RChanClient";
    import Captcha from "../Captcha.svelte";
    import ErrorValidacion from "../ErrorValidacion.svelte";
    import config from "../../config"

    let token = window.model.token


</script>
<div class="fondo"></div>
<main>
    <!-- <video src="623eb91fd792a152481ebe7cecc2ce9f.mp4" loop autoplay muted></video> -->

    <section style="word-break: break-all;">
        <h2>Este es tu token de sesion</h2>
        <p>{token}</p>
        <h4>Sirve para poder usar la misma sesion en diferentes navegadores y dispositivos</h4>
        <p>Para utilizarlo tenes que dirigirte al <a href="/Login">login</a> y cliquear la opcion usar token</p>
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
        display: flex;
        justify-content: center;
    }

    section {
        max-width: 100vh;
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