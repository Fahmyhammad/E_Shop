

     function CheckData() {
         if (document.getElementById("carrier").Value == '') {
             swal.fire({
                 title: 'There is an error',
                 text: "Please Enter Carrier ",
                 icon: 'error',
                 confirmButtonColor: '#308sd6',
                 cancelButtonColor: '#d33',
             });
             return false;
         }
         if (document.getElementById("tracking").Value == '') {
             swal.fire({
                 title: 'There is an error',
                 text: "Please Enter Tracking Number",
                 icon: 'error',
                 confirmButtonColor: '#308sd6',
                 cancelButtonColor: '#d33',
             });
             return false;
         }
         return true;
     }
