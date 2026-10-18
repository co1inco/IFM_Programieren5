use crate::point2d::Point2D;

pub struct Obj {
    pub vertices: Vec<Point2D>,
    pub faces: Vec<Vec<usize>>,
}


pub fn parse_obj(file: &str) -> Obj {
    let file = std::fs::read_to_string(file).unwrap();

    let mut obj = Obj {
        vertices: vec![],
        faces: vec![],
    };

    for line in file.lines() {

        if (line.starts_with("v")) {

            let segments = line.split_ascii_whitespace();
            let f = segments
                .skip(1)
                .take(2)
                .map(|s| s.parse::<f32>().unwrap())
                .collect::<Vec<f32>>();

            obj.vertices.push(Point2D::new(f[0],f[1]))
        }
        else if line.starts_with("f") {

            let segments = line.split_ascii_whitespace();
            obj.faces.push(
                segments
                    .skip(1)
                    .map(|s| s.parse::<usize>().unwrap() - 1)
                    .collect::<Vec<usize>>()
            );
        }

    }

    return obj;
}