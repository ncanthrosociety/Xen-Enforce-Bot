 <?php

	if(!defined('USE_DATABASE')){

		echo '<img src="dbnoaccess.jpg" style="width:100%;height:100%">';
		die();

	}

	$host = "{{ xen_mysql_host }}";  // Cannot be localhost or new mysqli() will error.
	$user = "{{ xen_mysql_user_username }}";
	$password = "{{ xen_mysql_user_password }}";
	$dbdatabase = "{{ xen_mysql_database }}";
	$dbtable = "{{ xen_mysql_verify_table }}";

	if (!function_exists("resultToArray")) {
		function resultToArray($result) {
			$rows = array();
			while($row = $result->fetch_assoc()) {
				$rows[] = $row;
			}
			return $rows;
		}
	}

    $DB_OBJ = mysqli_init();
    $DB_OBJ->ssl_set(
		'{{ xen_mysql_client_key }}',
		'{{ xen_mysql_client_cert }}',
		'{{ xen_mysql_ca }}',
		NULL,
		NULL
	);
	$DB_OBJ->real_connect($host, $user, $password, $dbdatabase, NULL, NULL,
                          MYSQLI_CLIENT_SSL_DONT_VERIFY_SERVER_CERT)  // TODO IF insecure ... ELSE ...
                          or die('Failed to connect: ' . $DB_OBJ->error);
	$DB_OBJ->set_charset('utf8');

?>
