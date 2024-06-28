 <?php

	if(!defined('USE_DATABASE')){

		echo '<img src="dbnoaccess.jpg" style="width:100%;height:100%">';
		die();

	}

	$host = getenv("MYSQL_HOST"); // Cannot be localhost or new mysqli() will error.
	$user = getenv("MYSQL_USER_USERNAME");
	$password = getenv("MYSQL_USER_PASSWORD");
	$dbdatabase = getenv("MYSQL_DATABASE");
	$dbtable = getenv("MYSQL_VERIFY_TABLE");

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
		getenv('MYSQL_CLIENT_KEY'),
		getenv('MYSQL_CLIENT_CERT'),
		getenv('MYSQL_CA'),
		NULL,
		NULL
	);
	$DB_OBJ->real_connect($host, $user, $password, $dbdatabase, NULL, NULL,
                          MYSQLI_CLIENT_SSL_DONT_VERIFY_SERVER_CERT)  // TODO IF insecure ... ELSE ...
                          or die('Failed to connect: ' . $DB_OBJ->error);
	$DB_OBJ->set_charset('utf8');

?>
