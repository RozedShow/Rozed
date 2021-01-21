<script>
    import { Button, Checkbox } from "svelte-mui";
    import Dialogo from "./Dialogo.svelte";
    let colorPrincipal = "#17212b";
    let colorSecundario = "#cc1d1d";
    let colorSecundarioDefecto = "#cc1d1d"

    $: aplicarEstilo(colorPrincipal, colorSecundario)

    function aplicarEstilo(colorPrincipal, colorSecundario) {
        console.log("Cambiado color");
        // document.body.style.setProperty("background", hslToString(alterar(hexToHSL(colorPrincipal), +1, +5, -10)))
        // document.body.style.setProperty("--color1", colorPrincipal)
        // document.body.style.setProperty("--color2", colorPrincipal)
        // document.body.style.setProperty("--color3", hslToString(alterar(hexToHSL(colorPrincipal), +1, +5, + 10)))
        // document.body.style.setProperty("--color4", hslToString(alterar(hexToHSL(colorPrincipal), +1, -5, + 15)))
        document.body.style.setProperty("--color5", colorSecundario)
        // document.body.style.setProperty("--color1", colorPrincipal + ' !important')
    }

    $:color3 = hslToString(alterar(hexToHSL(colorPrincipal),10,10, -10))
    
    function alterar(colorHsl, h=0, s=0, l=0)
    {
        let ret = {}
        ret.l = Math.max(0,colorHsl.l + l)
        ret.s = Math.max(0,colorHsl.s + s)
        ret.h = Math.max(0,colorHsl.h + h)
        return ret
    }

    function hexToHSL(H) {
        // Convert hex to RGB first
        let r = 0,
            g = 0,
            b = 0;
        if (H.length == 4) {
            r = "0x" + H[1] + H[1];
            g = "0x" + H[2] + H[2];
            b = "0x" + H[3] + H[3];
        } else if (H.length == 7) {
            r = "0x" + H[1] + H[2];
            g = "0x" + H[3] + H[4];
            b = "0x" + H[5] + H[6];
        }
        // Then to HSL
        r /= 255;
        g /= 255;
        b /= 255;
        let cmin = Math.min(r, g, b),
            cmax = Math.max(r, g, b),
            delta = cmax - cmin,
            h = 0,
            s = 0,
            l = 0;

        if (delta == 0) h = 0;
        else if (cmax == r) h = ((g - b) / delta) % 6;
        else if (cmax == g) h = (b - r) / delta + 2;
        else h = (r - g) / delta + 4;

        h = Math.round(h * 60);

        if (h < 0) h += 360;

        l = (cmax + cmin) / 2;
        s = delta == 0 ? 0 : delta / (1 - Math.abs(2 * l - 1));
        s = +(s * 100).toFixed(1);
        l = +(l * 100).toFixed(1);

        return {h,s,l}
    }

    
    function hslToString(colorHsl) {
        return "hsl(" +  colorHsl.h + "," + colorHsl.s + "%," + colorHsl.l + "%)"
    }
</script>

<svelte:body style="--color5:blue !important" />

<Dialogo>
    <div class="" slot="body">
        Color: <input bind:value={colorSecundario} type="color"  
            style="width: 2rem;height: 1.2rem;padding: 0;"/>
        <Checkbox right>Usar desplegable de cagetorias </Checkbox>
        <Checkbox right>Mutear videos </Checkbox>
    </div>
</Dialogo>

<!-- <div class="">
    selecionar color
    <br />
    color secundario :
    <input bind:value={colorSecundario} type="color" />

    <p style="background:{colorPrincipal}">color original</p>
    <p style="background:{color3}">colorMasOscuro</p>

    <Button>Aceptar</Button>
</div> -->
