package main

// apiserver is the api server for sea-apiserver service.
// it is responsible for serving the platform RESTful resource management.
import (
	"github.com/seacraft/internal/apiserver"
	"math/rand"
	"time"
)

func main() {
	rand.Seed(time.Now().UTC().UnixNano())
	apiserver.NewApp("sea-apiserver").Run()
}
