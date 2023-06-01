(function(window, undefined) {
  'use strict';

  /*
  NOTE:
  ------
  PLACE HERE YOUR OWN JAVASCRIPT CODE IF NEEDED
  WE WILL RELEASE FUTURE UPDATES SO IN ORDER TO NOT OVERWRITE YOUR JAVASCRIPT CODE PLEASE CONSIDER WRITING YOUR SCRIPT HERE.  */

	$(document).ready(function () {
		$('#Sites_dropdown').multiselect({
			includeSelectAllOption: true,
			includeSelectOption: true,
			buttonWidth: '200px'
		});
	});

})(window);