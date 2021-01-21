<script>
    import { onMount } from "svelte";
    import { configStore } from "../../config"
    let activo = false

    $: activo = $configStore.general.flags.includes("[3:33]")

    $: tembleque = Math.random() > 0.5 && activo

    let audio = new Audio("/audio/re4regenerador.mp3")
    audio.loop = true
    $: tembleque? audio.volume = 1 : audio.volume = 0
    $:if(activo) {
        try {
            audio.play()
        } catch (error) {
            console.log(error)
        }
    }


    setInterval(() => {
    },1000)

    

    $: if(tembleque) document.body.classList.add("nls")
        else document.body.classList.remove("nls")

    onMount(async () => {
        tembleque = Math.random() > 0.5 && activo
        while (true) {
            if(activo) {
                await wait(Math.random() * 10 * 1000)
                tembleque = true
                await wait(Math.random() * 10 * 1000)
                tembleque = false
            } else {
                await wait(Math.random() * 10 * 1000)
                tembleque = false
            }
        }
    })

    function wait(milisegundos = 1000) {
        return new Promise((resolve) => {
            setTimeout(resolve, milisegundos)
        })
    }
</script>

<style>
    @keyframes  -global-temblequeo{
        0% {
            transform: translateY(0px);
            background: red !important;
        }
        50% {
            transform: translateY(50px) rotateY(23deg)
        }
        100% {
            transform: translateY(0px)
        }
    }
    :global(.nls) {
        filter: brightness(0.7) contrast(2.5) saturate(4.5);
        animation: temblequeo 0.1s infinite;
    }
</style>