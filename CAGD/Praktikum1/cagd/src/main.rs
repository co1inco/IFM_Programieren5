mod point2d;
mod vector2d;
mod obj_parser;

use bevy::prelude::*;
use crate::obj_parser::{parse_obj, Obj};
use crate::point2d::Point2D;
use crate::vector2d::Vector2D;

const POINT_SCALE: f32 = 40.0;
const POINT_RADIUS: f32 = 8.0;

#[derive(Component)]
#[allow(dead_code)]
pub struct FacePoint {
    pub point: Point2D,
}

fn setup_camera(mut commands: Commands) {
    commands.spawn(Camera2d);
}

fn face_center(obj: &Obj, face_idx: usize) -> Option<Point2D> {

    let Some(face) = obj.faces.get(face_idx) else { return None; };

    let mut avg = Vector2D::default();
    let mut count = 0;
    for &vertex_idx in face {
        if let Some(vertex) = obj.vertices.get(vertex_idx) {
            avg = avg + vertex.to_vector();
            count += 1;
        }
    }

    return Some((avg / count as f32).to_point());
}


fn setup_obj(
    mut commands: Commands,
    mut meshes: ResMut<Assets<Mesh>>,
    mut materials: ResMut<Assets<ColorMaterial>>,
) {
    let mut obj = parse_obj("resources/Curve_1.obj");

    let mesh_handle = meshes.add(Circle::new(POINT_RADIUS));
    let material_handle = materials.add(Color::srgb(0.9, 0.2, 0.2));
    let center_material = materials.add(Color::srgb(0.1, 0.9, 0.9));

    commands.spawn((
        FacePoint { point: Point2D::new(0.0, 0.0) },
        Mesh2d(mesh_handle.clone()),
        MeshMaterial2d(center_material.clone()),
    ));

    for (face_idx, face) in obj.faces.iter().enumerate() {
        println!("Face {}: {:?}", face_idx, face);

        let center = face_center(&obj, face_idx).unwrap().to_vector();


        for &vertex_idx in face {
            if (vertex_idx >= obj.vertices.len()) {
                continue;
            }

            let l = obj.vertices[vertex_idx].to_vector() - center;
            obj.vertices[vertex_idx] = l.to_point();

            if let Some(vertex) = obj.vertices.get(vertex_idx) {

                println!("  Point (vertex {}): {:?}", vertex_idx + 1, vertex);

                // Draw points
                commands.spawn((
                    FacePoint { point: *vertex },
                    Mesh2d(mesh_handle.clone()),
                    MeshMaterial2d(material_handle.clone()),
                    Transform::from_xyz(
                        (vertex.x) * POINT_SCALE,
                        (vertex.y) * POINT_SCALE,
                        0.0
                    ),
                ));
            }
        }
    }

    commands.insert_resource(obj);
}

fn draw_control_lines(obj: Option<Res<Obj>>, mut gizmos: Gizmos) {
    if let Some(obj) = obj {
        for face in &obj.faces {
            if face.len() < 2 {
                continue;
            }
            for i in 0..face.len()-1 {
                let v1 = obj.vertices[face[i]];
                let v2 = obj.vertices[face[(i + 1) % face.len()]];
                gizmos.line_2d(
                    Vec2::new(v1.x * POINT_SCALE, v1.y * POINT_SCALE),
                    Vec2::new(v2.x * POINT_SCALE, v2.y * POINT_SCALE),
                    Color::srgba(0.4, 0.6, 0.9, 0.6),
                );
            }
        }
    }
}

fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, (setup_camera, setup_obj))
        .add_systems(Update, draw_control_lines)
        .run();
}