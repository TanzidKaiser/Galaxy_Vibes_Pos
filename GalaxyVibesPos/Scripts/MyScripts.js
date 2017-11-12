

      $(document).ready(function () {

          $("#SubCategoryDropDwnForProductDetails").change(function () {

              //var subCategoryID = $(this).val();
              var subCategoryID = $("#SubCategoryDropDwnForProductDetails").val();

              $.getJSON("../Home/GetProductBySubCategory", { SubCategoryID: subCategoryID },

                     function (data) {

                         var select = $("#ProductDropdownForProductDetails");

                         select.empty();

                         select.append($('<option/>', {

                             value: 0,

                             text: "--Select Product--"

                         }));

                         $.each(data, function (index, itemData) {

                             select.append($('<option/>', {

                                 value: itemData.Value,

                                 text: itemData.Text

                             }));

                         });

                     });

          });


      });


