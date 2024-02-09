window.onclick = function(e) {
  if (!e.target.matches('.drop-button')) {
    let dropdowns = document.getElementsByClassName("drop-content");
    for (let i = 0; i < dropdowns.length; i++) {
      if (dropdowns[i].classList.contains('drop-show')) {
        dropdowns[i].classList.remove('drop-show');
      }
    }
  }
}


function dropdown(element) {
  switch (element)
  {
    case "dropdown1":
      document.getElementById("dropdown1").classList.toggle('drop-show');
      document.getElementById("dropdown2").classList.remove('drop-show');
    default:
      document.getElementById("dropdown2").classList.toggle('drop-show');
      document.getElementById("dropdown1").classList.remove('drop-show');
  }
}
