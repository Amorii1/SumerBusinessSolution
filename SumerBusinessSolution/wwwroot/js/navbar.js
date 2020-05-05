const hamburger = document.querySelector(".hamburger");
const sideBar = document.querySelector(".wrapper .sidebar");
const links = document.querySelectorAll(".wrapper .sidebar li");

hamburger.addEventListener("click", () => {
    sideBar.classList.toggle("open");
});