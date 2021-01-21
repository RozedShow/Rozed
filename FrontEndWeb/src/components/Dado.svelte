
<div class="fifth-face dice">
  
    <div class="column">
      <span class="dot"></span>
      <span class="dot"></span>
    </div>
    
    <div class="column">
      <span class="dot"></span>
    </div>
    
    <div class="column">
      <span class="dot"></span>
      <span class="dot"></span>
    </div>
  
  </div>

  <style>
/* Intial setup for displaying things in center */


/* code starts here */


.dice {
  padding: 4px;
  background-color: var(--color6);;
  width: 18px;
  height: 18px;
  margin: 0 2px;
  border-radius: 10%;
}
.dot{
  display: block;
  width: 3px;
  height: 3px;
  border-radius: 50%;
  background-color:white;
}

 .fifth-face{
  display: flex;
  justify-content: space-between;
}
 .fifth-face .column{
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.fifth-face .column:nth-of-type(2) {
  justify-content: center;
}

  </style>