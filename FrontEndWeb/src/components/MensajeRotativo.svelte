<script>
    import { onMount } from 'svelte';
    export let texto = `Me agarré a las piñas con la familia de mi novia

Fui a cenar a lo de mi novia y me agarre con el padre.
Les pasó a contar, la cosa es que estoy saliendo hace dos meses con una chica y el sábado pasado fui a cenar a la casa por primera vez.
La cita era en la casa del padre a eso de las 21,00hs. Apenas llego nos atiende una chica de más de 20 años a lo que le pregunto si era la hermana, la chica puso cara rara y me dijo "no, soy la novia del papá" a esto el padre estaba ya sentado en la cabeza de la mesa y escuchó todo, me clavó una mirada de odio terrible. Ahí nomás me presenta con el padre y el hermano que también estaba ahí.
Todo transcurria con normalidad hasta que me ofrece pan para la comida (hicieron ravioles caseros) lo cual le digo que no, gracias,que estoy dejando los carbohidratos y que ya tenía sufriente con los ravioles, a lo que el padre se me caga de risa y me dice"pero si pareces una lombriz, mi nena es mucha carne para tan poco perro". En ese instante me sale del alma " que maleducado que es usted"a lo cual instantaneamente se levanta y dice "que dijiste pendejo de mierda" y me quiere dar un cachetazo, al cual por reflejo se lo freno apartandole la mano violentamente. Al toque salta el hermano y nose que me dice, lo miro y el padre me emboca una piña en la cara, ahí me caigo un poco para atrás se caen unos vasos los platos, me repongo y le devuelvo la piña, se la pego pero de refilón,y ahí salta el hermano a pegarme a traición, lo veo venir con mi vista periférica y trato de esquivarlo, a lo cual la hermana me quiere dar un botellazo.
Esquivo el botellazo y me quiere pegar de nuevo, ahí me doy cuenta que los tres están contra mi, entonces no me quedo otra que, lo tenía que hacer, hago una maniobra para pararme bien y hago la patada 360° sobre mi propio eje, golpeando a los tres a la vez,agarró a mi novia salimos corriendo y pasamos toda la noche haciendo el amor`
    let textWidth = 0
    let rt1
    let rt2
    let width = 100
    let duracion = 100
    $: if(width && rt1  && rt2) {
        duracion =  (width  * width)/ 2000
        rt1.classList.remove("rt1")
        rt2.classList.remove("rt2")
        setTimeout(() => {
            rt1.classList.add("rt1")
            rt2.classList.add("rt2")
        },50)
    }


</script>
<div bind:offsetWidth={width} class="container">
    <div class="mensaje-rotativo" style="--width: {textWidth}px; --duracion: {duracion}s">
        <span bind:this={rt1} bind:offsetWidth={textWidth} class="rt1">{texto}</span>
        <span bind:this={rt2} class="rt2">{texto}</span>
    </div>
</div>

<style>
.container {
    position: relative;
    width: 100%;
    /* height: 20px; */
    display: flex;
    align-items: center;
}
.mensaje-rotativo {
    position: absolute;
    clip: rect(auto, auto, auto, auto);
    height: 20px;
    width: 100%;
}
@keyframes rotativo {
    from{transform: translate(var(--width));}
    to{transform: translate(0);}
}
@keyframes rotativo2 {
    from{transform: translate(0);}
  to{transform: translate(calc(var(--width) * -1));}
}

.mensaje-rotativo span {
    position:absolute;
    animation-duration: var(--duracion);
    animation-timing-function: linear;
    animation-iteration-count: infinite;
    color: white;
    width: max-content;


}
.rt1 {
  animation-name: rotativo;
}

.rt2 {
  animation-name: rotativo2;
  color: red;
}
</style>